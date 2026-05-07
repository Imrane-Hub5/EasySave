using System;
using System.Collections.Generic;
using System.IO;
using EasyLog;
using EasySave.Services;
using EasySave.Strategies;

namespace EasySave.Models
{
    /// <summary>
    /// Represents a backup job configuration: name, source/target paths, and backup type.
    /// Uses the Strategy pattern to delegate the actual copy logic at runtime.
    /// </summary>
    public class BackupJob
    {
        public string Name { get; set; } = string.Empty;
        public string SourcePath { get; set; } = string.Empty;
        public string TargetPath { get; set; } = string.Empty;
        public BackupType Type { get; set; }

        private IBackupStrategy? _strategy;

        // Parameterless constructor required for JSON deserialization
        public BackupJob() { }

        public BackupJob(string name, string sourcePath, string targetPath, BackupType type)
        {
            Name = name;
            SourcePath = sourcePath;
            TargetPath = targetPath;
            Type = type;
        }

        /// <summary>
        /// Injects the copy strategy (full or differential) before execution.
        /// </summary>
        public void SetStrategy(IBackupStrategy strategy)
        {
            _strategy = strategy;
        }

        /// <summary>
        /// Returns a default inactive state snapshot for this job.
        /// </summary>
        public StateEntry GetState() => new StateEntry { JobName = Name, Status = "Inactive" };

        /// <summary>
        /// Runs the backup: enumerates source files, copies each one via the strategy,
        /// logs the transfer, and updates the live state file after every file.
        /// </summary>
        public void Execute(Logger logger, StateManager stateManager)
        {
            if (_strategy == null) return;

            List<string> files = GetAllFiles(SourcePath);
            long totalSize = GetTotalSize(files);
            int remainingFiles = files.Count;
            long remainingSize = totalSize;

            // Write initial state before the first file is processed
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

                // Update state after each file so external tools see live progress
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

        /// <summary>
        /// Recursively collects all file paths under <paramref name="path"/>.
        /// Returns an empty list if the directory does not exist.
        /// </summary>
        private List<string> GetAllFiles(string path)
        {
            List<string> files = new List<string>();
            if (!Directory.Exists(path)) return files;
            files.AddRange(Directory.GetFiles(path));
            foreach (string dir in Directory.GetDirectories(path))
                files.AddRange(GetAllFiles(dir));
            return files;
        }

        /// <summary>
        /// Sums the byte size of every file in the list.
        /// </summary>
        private long GetTotalSize(List<string> files)
        {
            long total = 0;
            foreach (string file in files)
                total += new FileInfo(file).Length;
            return total;
        }
    }
}