using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using EasyLog;
using EasySave.Models;
using EasySave.Strategies;

namespace EasySave.Services
{
    /// <summary>
    /// Singleton - manages unlimited backup jobs
    /// </summary>
    public class BackupManager
    {
        private static BackupManager? _instance;
        private readonly List<BackupJob> _jobs = new();
        private readonly string _configPath;
        private readonly Logger _logger;
        private readonly StateManager _stateManager;

        private BackupManager()
        {
            string dir = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "EasySave"
            );
            Directory.CreateDirectory(dir);
            _configPath = Path.Combine(dir, "config.json");
            _logger = Logger.GetInstance();
            _stateManager = new StateManager();

            // Read log format directly from settings.json
            string settingsPath = Path.Combine(dir, "settings.json");
            string logFormat = "JSON";

            if (File.Exists(settingsPath))
            {
                try
                {
                    string settingsJson = File.ReadAllText(settingsPath);
                    JsonDocument doc = JsonDocument.Parse(settingsJson);
                    if (doc.RootElement.TryGetProperty("LogFormat", out JsonElement prop))
                        logFormat = prop.GetString() ?? "JSON";
                }
                catch { }
            }

            // Set formatter (Strategy Pattern)
            ILogFormatter formatter = logFormat == "XML"
                ? new XmlFormatter()
                : new JsonFormatter();
            _logger.SetFormatter(formatter);

            LoadConfig();
        }

        public static BackupManager GetInstance()
        {
            if (_instance == null)
                _instance = new BackupManager();
            return _instance;
        }

        public List<BackupJob> Jobs => _jobs;

        /// <summary>
        /// Adds a backup job
        /// </summary>
        public void AddJob(BackupJob job)
        {
            _jobs.Add(job);
            SaveConfig();
        }

        /// <summary>
        /// Removes a backup job by index
        /// </summary>
        public bool RemoveJob(int index)
        {
            if (index < 0 || index >= _jobs.Count) return false;
            _jobs.RemoveAt(index);
            SaveConfig();
            return true;
        }

        /// <summary>
        /// Runs a single backup job
        /// </summary>
        public void RunJob(int index)
        {
            if (index < 0 || index >= _jobs.Count) return;
            BackupJob job = _jobs[index];
            AssignStrategy(job);
            job.ExecuteAsync(_logger, _stateManager).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Runs all backup jobs in parallel using Task.WhenAll()
        /// </summary>
        public void RunAll()
        {
            List<Task> tasks = new();

            foreach (BackupJob job in _jobs)
            {
                BackupJob current = job;
                AssignStrategy(current);
                tasks.Add(current.ExecuteAsync(_logger, _stateManager));
            }

            Task.WhenAll(tasks).Wait();
        }

        /// <summary>
        /// Runs jobs from command line args (ex: 1-3 or 1;3)
        /// </summary>
        public void ExecuteRange(string args)
        {
            if (args.Contains("-"))
            {
                string[] parts = args.Split("-");
                if (int.TryParse(parts[0], out int start) &&
                    int.TryParse(parts[1], out int end))
                {
                    for (int i = start - 1; i < end && i < _jobs.Count; i++)
                        RunJob(i);
                }
            }
            else if (args.Contains(";"))
            {
                foreach (string part in args.Split(";"))
                    if (int.TryParse(part.Trim(), out int index))
                        RunJob(index - 1);
            }
            else if (int.TryParse(args.Trim(), out int single))
            {
                RunJob(single - 1);
            }
        }

        /// <summary>
        /// Assigns the correct strategy to a job
        /// </summary>
        private void AssignStrategy(BackupJob job)
        {
            if (job.Type == BackupType.Complete)
                job.SetStrategy(new FullBackupStrategy());
            else
                job.SetStrategy(new DiffBackupStrategy());
        }

        /// <summary>
        /// Saves jobs to config.json
        /// </summary>
        public void SaveConfig()
        {
            string json = JsonSerializer.Serialize(_jobs, new JsonSerializerOptions
            {
                WriteIndented = true
            });
            File.WriteAllText(_configPath, json);
        }

        /// <summary>
        /// Loads jobs from config.json
        /// </summary>
        public void LoadConfig()
        {
            if (!File.Exists(_configPath)) return;
            List<BackupJob>? jobs = JsonSerializer.Deserialize<List<BackupJob>>(
                File.ReadAllText(_configPath));
            if (jobs != null) _jobs.AddRange(jobs);
        }
    }
}