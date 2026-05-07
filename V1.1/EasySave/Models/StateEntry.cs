using System;

namespace EasySave.Models
{
    /// <summary>
    /// Represents the real-time state of a backup job
    /// </summary>
    public class StateEntry
    {
        public string JobName { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; }
        public string Status { get; set; } = string.Empty; // Active / Inactive
        public int TotalFiles { get; set; }
        public int RemainingFiles { get; set; }
        public long TotalSize { get; set; }
        public long RemainingSize { get; set; }
        public string SourceFile { get; set; } = string.Empty;
        public string TargetFile { get; set; } = string.Empty;
    }
}
