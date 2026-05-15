using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace EasySave.Services
{
    /// <summary>
    /// Manages priority file extensions.
    /// Blocks non-priority file transfers while priority files are pending.
    /// </summary>
    public static class PriorityQueue
    {
        private static readonly object _lock = new object();
        private static List<string> _priorityExtensions = new();

        /// <summary>
        /// Updates the list of priority extensions from Settings
        /// </summary>
        public static void SetExtensions(List<string> extensions)
        {
            lock (_lock)
            {
                _priorityExtensions = extensions
                    .Select(e => e.ToLowerInvariant().Trim())
                    .ToList();
            }
        }

        /// <summary>
        /// Returns true if the file has a priority extension
        /// </summary>
        public static bool IsPriority(string filePath)
        {
            string ext = Path.GetExtension(filePath).ToLowerInvariant();
            lock (_lock)
            {
                return _priorityExtensions.Contains(ext);
            }
        }

        /// <summary>
        /// Returns true if there are priority files pending in the given file list
        /// </summary>
        public static bool HasPendingPriorityFiles(IEnumerable<string> remainingFiles)
        {
            lock (_lock)
            {
                if (_priorityExtensions.Count == 0) return false;
                return remainingFiles.Any(f =>
                    _priorityExtensions.Contains(
                        Path.GetExtension(f).ToLowerInvariant()
                    ));
            }
        }

        /// <summary>
        /// Returns true if the file can be transferred now.
        /// A non-priority file is blocked if priority files are still pending.
        /// </summary>
        public static bool CanTransfer(string filePath, IEnumerable<string> remainingFiles)
        {
            if (IsPriority(filePath)) return true;
            return !HasPendingPriorityFiles(remainingFiles);
        }
    }
}
