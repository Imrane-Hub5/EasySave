using System;
using System.IO;

namespace EasyLog
{
    /// <summary>
    /// Singleton logger — delegates formatting to ILogFormatter (Strategy pattern)
    /// </summary>
    public class Logger
    {
        private static Logger? _instance;
        private readonly string _logDir;
        private ILogFormatter? _formatter;

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
        /// Sets the formatter to use (JSON or XML)
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
            if (_formatter == null)
                throw new InvalidOperationException("No formatter set. Call SetFormatter() first.");

            string filePath = Path.Combine(
                _logDir,
                DateTime.Now.ToString("yyyy-MM-dd") + _formatter.GetExtension()
            );

            string content = _formatter.Format(entry);
            File.AppendAllText(filePath, content + Environment.NewLine);
        }

        public string GetLogPath() => _logDir;
    }
}