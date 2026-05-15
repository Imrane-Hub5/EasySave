using System.Threading;

namespace EasySave.Services
{
    /// <summary>
    /// Controls a backup job execution — Pause, Play, Stop
    /// </summary>
    public class BackupJobController
    {
        private CancellationTokenSource _cts = new CancellationTokenSource();
        private ManualResetEventSlim _pauseEvent = new ManualResetEventSlim(true);

        public CancellationToken Token => _cts.Token;

        /// <summary>
        /// Pauses the backup job after current file finishes
        /// </summary>
        public void Pause() => _pauseEvent.Reset();

        /// <summary>
        /// Resumes the backup job
        /// </summary>
        public void Play() => _pauseEvent.Set();

        /// <summary>
        /// Stops the backup job immediately
        /// </summary>
        public void Stop() => _cts.Cancel();

        /// <summary>
        /// Waits if paused — called at each file iteration in BackupJob
        /// </summary>
        public void WaitIfPaused() => _pauseEvent.Wait();

        /// <summary>
        /// Resets the controller for reuse
        /// </summary>
        public void Reset()
        {
            _cts = new CancellationTokenSource();
            _pauseEvent = new ManualResetEventSlim(true);
        }
    }
}