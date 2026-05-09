using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using EasyLog;
using EasySave.Models;
using EasySave.Strategies;

namespace EasySave.Services
{
    /// <summary>
    /// Singleton - manages all backup jobs (max 5)
    /// </summary>
    public class BackupManager
    {
        private static BackupManager? _instance;
        private readonly List<BackupJob> _jobs = new();
        private readonly int _maxJobs = 5;
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

            // Load log formatter from settings (Strategy Pattern)
            Settings settings = Settings.Load();
            ILogFormatter formatter = settings.LogFormat == "XML"
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

        public bool AddJob(BackupJob job)
        {
            if (_jobs.Count >= _maxJobs) return false;
            _jobs.Add(job);
            SaveConfig();
            return true;
        }

        public bool RemoveJob(int index)
        {
            if (index < 0 || index >= _jobs.Count) return false;
            _jobs.RemoveAt(index);
            SaveConfig();
            return true;
        }

        public void RunJob(int index)
        {
            if (index < 0 || index >= _jobs.Count) return;
            BackupJob job = _jobs[index];
            AssignStrategy(job);
            job.Execute(_logger, _stateManager);
        }

        public void RunAll()
        {
            foreach (BackupJob job in _jobs)
            {
                AssignStrategy(job);
                job.Execute(_logger, _stateManager);
            }
        }

        public void ExecuteRange(string args)
        {
            if (args.Contains("-"))
            {
                string[] parts = args.Split("-");
                if (int.TryParse(parts[0], out int start) && int.TryParse(parts[1], out int end))
                    for (int i = start - 1; i < end && i < _jobs.Count; i++)
                        RunJob(i);
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

        private void AssignStrategy(BackupJob job)
        {
            if (job.Type == BackupType.Complete)
                job.SetStrategy(new FullBackupStrategy());
            else
                job.SetStrategy(new DiffBackupStrategy());
        }

        public void SaveConfig()
        {
            string json = JsonSerializer.Serialize(_jobs, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_configPath, json);
        }

        public void LoadConfig()
        {
            if (!File.Exists(_configPath)) return;
            List<BackupJob>? jobs = JsonSerializer.Deserialize<List<BackupJob>>(File.ReadAllText(_configPath));
            if (jobs != null) _jobs.AddRange(jobs);
        }
    }
}