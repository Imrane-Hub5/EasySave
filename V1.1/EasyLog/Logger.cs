using System;
using System.IO;
using System.Text.Json;

namespace EasyLog
{
    /// <summary>
    /// Singleton logger - writes daily JSON log files
    /// </summary>
    public class Logger
    {
        private static Logger? _instance;
        private readonly string _logDir;

        private Logger()
        {
            _logDir = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "EasySave", "Logs"
            );
            Directory.CreateDirectory(_logDir);
        }

        public static Logger GetInstance()
        {
            if (_instance == null)
                _instance = new Logger();
            return _instance;
        }

        /// <summary>
        /// Writes a log entry to the daily JSON log file
        /// </summary>
        public void Log(LogEntry entry)
        {
            string filePath = Path.Combine(_logDir, DateTime.Now.ToString("yyyy-MM-dd") + ".json");
            string json = JsonSerializer.Serialize(entry, new JsonSerializerOptions { WriteIndented = true });
            File.AppendAllText(filePath, json + Environment.NewLine);
        }
    }
}
