using System;
using System.Diagnostics;
using System.IO;

namespace EasySave
{
    /// <summary>
    /// Full backup strategy - copies all files regardless of modification date
    /// </summary>
    public class FullBackupStrategy : IBackupStrategy
    {
        public string GetTypeName() => "Complete";

        /// <summary>
        /// Copies the file from source to target
        /// </summary>
        /// <returns>Transfer time in ms, negative if error</returns>
        public long Execute(string src, string dst)
        {
            try
            {
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
