using System;
using System.Diagnostics;
using System.IO;

namespace EasySave.Strategies
{
    public class DiffBackupStrategy : IBackupStrategy
    {
        public string GetTypeName() => "Differential";

        public long Execute(string src, string dst)
        {
            try
            {
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