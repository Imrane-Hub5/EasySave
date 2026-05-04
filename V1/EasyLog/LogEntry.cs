using System;

namespace EasyLog
{
    /// <summary>
    /// Represents a single log entry for a file transfer
    /// </summary>
    public class LogEntry
    {
        public DateTime Timestamp { get; set; }
        public string JobName { get; set; } = string.Empty;
        public string SourcePath { get; set; } = string.Empty;
        public string TargetPath { get; set; } = string.Empty;
        public long FileSize { get; set; }
        public long TransferTime { get; set; } // negative if error
    }
}
