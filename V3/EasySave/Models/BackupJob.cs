using System;
using System.Collections.Generic;
using System.IO;
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
        /// Executes the backup asynchronously with parallel and large-file constraint supports
        /// </summary>
        public async Task ExecuteAsync(Logger logger, StateManager stateManager)
        {
            if (_strategy == null) return;

            Settings settings = Settings.Load();
            CryptoSoftService crypto = new CryptoSoftService(settings.CryptoSoftPath);

            // Check business software before starting from global flag managed by monitoring thread
            if (BusinessSoftwareService.IsBlocked)
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
                if (Controller.Token.IsCancellationRequested)
                {
                    stateManager.ResetState(Name);
                    return;
                }

                Controller.WaitIfPaused();

                // Check global business software flag during loop execution
                if (BusinessSoftwareService.IsBlocked)
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
