using System;
using System.Collections.Generic;
using System.IO;
using EasyLog;

namespace EasySave
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

        private IBackupStrategy? _strategy;

        public BackupJob() { }

        public BackupJob(string name, string sourcePath, string targetPath, BackupType type)
        {
            Name = name;
            SourcePath = sourcePath;
            TargetPath = targetPath;
            Type = type;
        }

        public void SetStrategy(IBackupStrategy strategy)
        {
            _strategy = strategy;
        }

        /// <summary>
        /// Returns current state of the job
        /// </summary>
        public JobState GetState() => new JobState { JobName = Name, Status = "Inactive" };

        /// <summary>
        /// Executes the backup using the assigned strategy
        /// </summary>
        public void Execute(Logger logger, StateManager stateManager)
        {
            if (_strategy == null) return;

            List<string> files = GetAllFiles(SourcePath);
            long totalSize = GetTotalSize(files);
            int remainingFiles = files.Count;
            long remainingSize = totalSize;

            stateManager.UpdateState(new JobState
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
                string relativePath = Path.GetRelativePath(SourcePath, sourceFile);
                string targetFile = Path.Combine(TargetPath, relativePath);
                long fileSize = new FileInfo(sourceFile).Length;

                long transferTime = _strategy.Execute(sourceFile, targetFile);

                logger.Log(new LogEntry
                {
                    Timestamp = DateTime.Now,
                    JobName = Name,
                    SourcePath = sourceFile,
                    TargetPath = targetFile,
                    FileSize = fileSize,
                    TransferTime = transferTime
                });

                remainingFiles--;
                remainingSize -= fileSize;

                stateManager.UpdateState(new JobState
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
