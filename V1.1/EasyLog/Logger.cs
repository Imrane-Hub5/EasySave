using System;
using System.IO;

namespace EasyLog
{
    /// <summary>
    /// Singleton logger - writes daily log files using a formatter
    /// </summary>
    public class Logger
    {
        private static Logger? _instance;
        private readonly string _logDir;
        private readonly ILogFormatter _formatter;

        private Logger(ILogFormatter formatter)
        {
            _formatter = formatter;
            _logDir = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "EasySave", "Logs"
            );
            Directory.CreateDirectory(_logDir);
        }

        public static Logger GetInstance(ILogFormatter? formatter = null)
        {
            if (_instance == null)
                _instance = new Logger(formatter ?? new JsonFormatter());
            return _instance;
        }

        /// <summary>
        /// Writes a log entry to the daily log file using the configured formatter
        /// </summary>
        public void Log(LogEntry entry)
        {
            string filePath = Path.Combine(_logDir, DateTime.Now.ToString("yyyy-MM-dd") + _formatter.FileExtension);
            string content = _formatter.Format(entry);
            File.AppendAllText(filePath, content + Environment.NewLine);
        }
    }
}