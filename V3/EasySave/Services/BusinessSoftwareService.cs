using System.Diagnostics;
using System.Linq;

namespace EasySave.Services
{
    public class BusinessSoftwareService
    {
        public bool IsBusinessSoftwareRunning(string processName)
        {
            if (string.IsNullOrWhiteSpace(processName)) return false;

            // Returns true if any process matches the name (e.g., "CalculatorApp" or "calc")
            return Process.GetProcessesByName(processName).Any();
        }
    }
}
