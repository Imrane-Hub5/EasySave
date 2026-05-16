using System;
using System.IO;

namespace EasyLog
{
    /// <summary>
    /// Singleton logger - writes daily log files.
    /// Supports three destination modes: Local, Remote, or Both.
    /// </summary>
    public class Logger
    {
        private static Logger? _instance;
        private readonly string _logDir;
        private ILogFormatter _formatter = new JsonFormatter();
        private LogDestination _destination = LogDestination.Local;
        private string _remoteLogDir = string.Empty;

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
        /// Sets the log formatter (JSON or XML).
        /// </summary>
        public void SetFormatter(ILogFormatter formatter)
        {
            _formatter = formatter;
        }

        /// <summary>
        /// Sets the log destination mode (Local, Remote, or Both).
        /// </summary>
        public void SetDestination(LogDestination destination, string remoteDir = "")
        {
            _destination = destination;
            _remoteLogDir = remoteDir;

            if (_destination != LogDestination.Local && !string.IsNullOrEmpty(_remoteLogDir))
                Directory.CreateDirectory(_remoteLogDir);
        }

        /// <summary>
        /// Writes a log entry according to the current destination mode.
        /// </summary>
        public void Log(LogEntry entry)
        {
            string ext = _formatter.GetExtension();
            string fileName = DateTime.Now.ToString("yyyy-MM-dd") + "." + ext;
            string content = _formatter.Format(entry) + Environment.NewLine;

            if (_destination == LogDestination.Local || _destination == LogDestination.Both)
                WriteToFile(Path.Combine(_logDir, fileName), content);

            if (_destination == LogDestination.Remote || _destination == LogDestination.Both)
                if (!string.IsNullOrEmpty(_remoteLogDir))
                    WriteToFile(Path.Combine(_remoteLogDir, fileName), content);
        }

        /// <summary>
        /// Writes content to a file.
        /// </summary>
        private void WriteToFile(string filePath, string content)
        {
            File.AppendAllText(filePath, content);
        }

        /// <summary>
        /// Returns the local log directory path.
        /// </summary>
        public string GetLogPath() => _logDir;
    }

    /// <summary>
    /// Log destination mode — Local, Remote, or Both.
    /// </summary>
    public enum LogDestination
    {
        Local,
        Remote,
        Both
    }
}