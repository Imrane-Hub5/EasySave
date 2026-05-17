using System.Threading;

namespace EasySave.Services
{
    public class BackupSemaphore
    {
        private static SemaphoreSlim _largeFileSemaphore = new SemaphoreSlim(1, 1);

        public static async Task AccessLargeFile(long fileSize, Func<Task> action)
        {
            Settings settings = Settings.Load();
            long threshold = settings.MaxParallelFileSizeKo * 1024;

            if (fileSize > threshold)
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
