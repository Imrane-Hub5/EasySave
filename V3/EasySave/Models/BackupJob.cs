using System;
using System.Collections.Generic;
using System.IO;
using EasyLog;
using EasySave.Services;
using EasySave.Strategies;

namespace EasySave.Models
{
    /// <summary>
    /// Represents a single backup job
    /// </summary>
    public class BackupJob
    {
        public string Name { get; set; } = string.Empty;
        public string SourcePath { get; set; } = string.Empty;
        public string TargetPath { get; set; } = string.Empty;
        public BackupType Type { get; set; }

        public BackupJobController Controller { get; } = new BackupJobController();

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
        /// Executes the backup — supports Pause/Play/Stop via BackupJobController
        /// </summary>
        public void Execute(Logger logger, StateManager stateManager)
        {
            if (_strategy == null) return;

            Settings settings = Settings.Load();
            BusinessSoftwareService bss = new BusinessSoftwareService();
            CryptoSoftService crypto = new CryptoSoftService(settings.CryptoSoftPath);

            // Check business software before starting
            if (bss.IsBusinessSoftwareRunning(settings.BusinessSoftware))
            {
                logger.Log(new LogEntry
                {
                    Timestamp = DateTime.Now,
                    JobName = Name,
                    SourcePath = string.Empty,
                    TargetPath = string.Empty,
                    FileSize = 0,
                    TransferTime = -1,
                    EncryptionTime = 0
                });
                return;
            }

            Controller.Reset();

            List<string> files = GetAllFiles(SourcePath);
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

            foreach (string sourceFile in files)
            {
                // Check stop
                if (Controller.Token.IsCancellationRequested)
                {
                    stateManager.ResetState(Name);
                    return;
                }

                // Check pause
                Controller.WaitIfPaused();

                // Check business software during execution
                if (bss.IsBusinessSoftwareRunning(settings.BusinessSoftware))
                {
                    logger.Log(new LogEntry
                    {
                        Timestamp = DateTime.Now,
                        JobName = Name,
                        SourcePath = sourceFile,
                        TargetPath = string.Empty,
                        FileSize = 0,
                        TransferTime = -1,
                        EncryptionTime = 0
                    });
                    stateManager.ResetState(Name);
                    return;
                }

                string relativePath = Path.GetRelativePath(SourcePath, sourceFile);
                string targetFile = Path.Combine(TargetPath, relativePath);
                long fileSize = new FileInfo(sourceFile).Length;

                long transferTime = _strategy.Execute(sourceFile, targetFile);

                long encryptionTime = 0;
                if (crypto.ShouldEncrypt(sourceFile, settings.EncryptedExtensions ?? new List<string>()))
                    encryptionTime = crypto.EncryptFile(targetFile);

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
            }

            stateManager.ResetState(Name);
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