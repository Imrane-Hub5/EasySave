using System;
using System.IO;

namespace EasyLog
{
    /// <summary>
    /// Singleton logger - writes daily log files
    /// </summary>
    public class Logger
    {
        private static Logger? _instance;
        private readonly string _logDir;
        private ILogFormatter _formatter = new JsonFormatter();

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
        /// Sets the log formatter (JSON or XML)
        /// </summary>
        public void SetFormatter(ILogFormatter formatter)
        {
            _formatter = formatter;
        }

        /// <summary>
        /// Writes a log entry using the current formatter
        /// </summary>
        public void Log(LogEntry entry)
        {
            string ext = _formatter.GetExtension();
            string filePath = Path.Combine(_logDir, DateTime.Now.ToString("yyyy-MM-dd") + "." + ext);
            string content = _formatter.Format(entry);
            File.AppendAllText(filePath, content + Environment.NewLine);
        }
    }
}