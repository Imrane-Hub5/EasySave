using System.Diagnostics;
using System.Linq;

namespace EasySave.Services
{
    public class BusinessSoftwareService
    {
        // Global flag accessible by all running BackupJobs in real-time
        public static bool IsBlocked { get; private set; }

        public bool IsBusinessSoftwareRunning(string processName)
        {
            if (string.IsNullOrWhiteSpace(processName)) return false;

            return Process.GetProcessesByName(processName).Any();
        }

        // Method called by the monitoring thread to update the global state
        public void MonitorProcess(string processName)
        {
            IsBlocked = IsBusinessSoftwareRunning(processName);
        }
    }
}
