using System;
using System.Diagnostics;
using System.IO;

namespace EasySave.Strategies
{
    /// <summary>
    /// Differential backup strategy - only copies files that are newer in the source
    /// than their counterpart in the target directory.
    /// </summary>
    public class DiffBackupStrategy : IBackupStrategy
    {
        public string GetTypeName() => "Differential";

        /// <summary>
        /// Copies the file only if it has been modified since the last backup.
        /// Skips the copy and returns 0 when the destination is already up to date.
        /// </summary>
        /// <returns>Transfer time in ms, 0 if skipped, negative if error.</returns>
        public long Execute(string src, string dst)
        {
            try
            {
                // Skip if destination exists and is at least as recent as the source
                if (File.Exists(dst) && File.GetLastWriteTime(src) <= File.GetLastWriteTime(dst))
                    return 0;

                string? dir = Path.GetDirectoryName(dst);
                if (dir != null) Directory.CreateDirectory(dir);

                Stopwatch sw = Stopwatch.StartNew();
                File.Copy(src, dst, overwrite: true);
                sw.Stop();
                return sw.ElapsedMilliseconds;
            }
            catch (Exception)
            {
                return -1;
            }
        }
    }
}