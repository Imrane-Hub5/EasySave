using System.Threading;

namespace EasySave.Services
{
    public class BackupSemaphore
    {
        private static SemaphoreSlim _largeFileSemaphore = new SemaphoreSlim(1, 1);
        private const long LargeFileThreshold = 15 * 1024 * 1024; // 15 MB

        public static async Task AccessLargeFile(long fileSize, Func<Task> action)
        {
            if (fileSize > LargeFileThreshold)
            {
                await _largeFileSemaphore.WaitAsync();
                try { await action(); }
                finally { _largeFileSemaphore.Release(); }
            }
            else
            {
                await action();
            }
        }
    }
}
