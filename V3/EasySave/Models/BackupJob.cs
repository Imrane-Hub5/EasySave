using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using EasyLog;
using EasySave.Services;
using EasySave.Strategies;

namespace EasySave.Models
{
    public class BackupJob
    {
        public string Name { get; set; } = string.Empty;
        public string SourcePath { get; set; } = string.Empty;
        public string TargetPath { get; set; } = string.Empty;
        public BackupType Type { get; set; }

        [JsonIgnore]
        public BackupJobController Controller { get; } = new BackupJobController();

        // (progressPercent 0-100, status)
        public event Action<double, string>? ProgressChanged;

        private IBackupStrategy? _strategy;

        public BackupJob() { }

        public BackupJob(string name, string sourcePath, string targetPath, BackupType type)
        {
            Name = name;
            SourcePath = sourcePath;
            TargetPath = targetPath;
            Type = type;
        }

        public void SetStrategy(IBackupStrategy strategy) => _strategy = strategy;

        public StateEntry GetState() => new StateEntry { JobName = Name, Status = "Inactive" };

        /// <summary>
        /// Executes the backup asynchronously with parallel and large-file constraint supports
        /// </summary>
        public async Task ExecuteAsync(Logger logger, StateManager stateManager)
        {
            if (_strategy == null) return;

            Settings settings = Settings.Load();
            CryptoSoftService crypto = new CryptoSoftService(settings.CryptoSoftPath);
            PriorityQueue.SetExtensions(settings.PriorityExtensions ?? new List<string>());
            BusinessSoftwareService.Configure(settings.BusinessSoftware);

            // Wait while business software is running before starting
            if (BusinessSoftwareService.IsBlocked)
            {
                ProgressChanged?.Invoke(0, "Paused");
                while (BusinessSoftwareService.IsBlocked)
                    await Task.Delay(500);
            }

            Controller.Reset();

            // Sort files: priority extensions first, then others
            List<string> files = GetAllFiles(SourcePath)
                .OrderByDescending(f => PriorityQueue.IsPriority(f))
                .ToList();
            long totalSize = GetTotalSize(files);
            int remainingFiles = files.Count;
            long remainingSize = totalSize;

            stateManager.UpdateState(new StateEntry
            {
                JobName = Name,
                Status = "Active",
                TotalFiles = files.Count,
                TotalSize = totalSize,
                RemainingFiles = remainingFiles,
                RemainingSize = remainingSize
            });
            ProgressChanged?.Invoke(0, "Active");

            foreach (string sourceFile in files)
            {
                if (Controller.Token.IsCancellationRequested)
                {
                    stateManager.ResetState(Name);
                    ProgressChanged?.Invoke(0, "Inactive");
                    return;
                }

                // Pause: block until Play() is called
                Controller.WaitIfPaused();

                // Business software running: signal Paused, wait, then resume
                if (BusinessSoftwareService.IsBlocked)
                {
                    double curPct = totalSize > 0
                        ? (double)(totalSize - remainingSize) / totalSize * 100.0
                        : 0;
                    ProgressChanged?.Invoke(curPct, "Paused");
                    while (BusinessSoftwareService.IsBlocked)
                    {
                        await Task.Delay(500);
                        Controller.WaitIfPaused();
                    }
                    ProgressChanged?.Invoke(curPct, "Active");
                }

                string relativePath = Path.GetRelativePath(SourcePath, sourceFile);
                string targetFile = Path.Combine(TargetPath, relativePath);
                long fileSize = new FileInfo(sourceFile).Length;

                long transferTime = 0;
                long encryptionTime = 0;

                // Use the static semaphore system to control concurrent transfers of large files
                await BackupSemaphore.AccessLargeFile(fileSize, async () =>
                {
                    // Run the potentially blocking I/O inside a Task block
                    transferTime = await Task.Run(() => _strategy.Execute(sourceFile, targetFile));

                    if (crypto.ShouldEncrypt(sourceFile, settings.EncryptedExtensions ?? new List<string>()))
                    {
                        encryptionTime = await Task.Run(() => crypto.EncryptFile(targetFile));
                    }
                });

                logger.Log(new LogEntry
                {
                    Timestamp = DateTime.Now,
                    JobName = Name,
                    SourcePath = sourceFile,
                    TargetPath = targetFile,
                    FileSize = fileSize,
                    TransferTime = transferTime,
                    EncryptionTime = encryptionTime
                });

                remainingFiles--;
                remainingSize -= fileSize;

                stateManager.UpdateState(new StateEntry
                {
                    JobName = Name,
                    Status = "Active",
                    TotalFiles = files.Count,
                    TotalSize = totalSize,
                    RemainingFiles = remainingFiles,
                    RemainingSize = remainingSize,
                    SourceFile = sourceFile,
                    TargetFile = targetFile
                });

                double pct = totalSize > 0
                    ? (double)(totalSize - remainingSize) / totalSize * 100.0
                    : 100.0;
                ProgressChanged?.Invoke(pct, "Active");
            }

            stateManager.ResetState(Name);
            ProgressChanged?.Invoke(100, "Inactive");
        }

        private List<string> GetAllFiles(string path)
        {
            List<string> files = new List<string>();
            if (!Directory.Exists(path)) return files;
            files.AddRange(Directory.GetFiles(path));
            foreach (string dir in Directory.GetDirectories(path))
                files.AddRange(GetAllFiles(dir));
            return files;
        }

        private long GetTotalSize(List<string> files)
        {
            long total = 0;
            foreach (string file in files)
                total += new FileInfo(file).Length;
            return total;
        }
    }
}
