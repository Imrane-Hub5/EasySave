using System;
using System.Diagnostics;
using System.Linq;

namespace EasySave.Services
{
    public class BusinessSoftwareService
    {
        private static string _processName = string.Empty;

        // Called at the start of each backup to pick up the latest setting
        public static void Configure(string processName)
        {
            processName = (processName ?? string.Empty).Trim();
            // GetProcessesByName does not use the .exe extension
            if (processName.EndsWith(".exe", StringComparison.OrdinalIgnoreCase))
                processName = processName[..^4];
            _processName = processName;
        }

        // True when the blocking software is currently running
        public static bool IsBlocked =>
            !string.IsNullOrEmpty(_processName) &&
            Process.GetProcessesByName(_processName).Any();
    }
}
