using System;
using System.IO;

namespace EasyLog
{
    public class Logger
    {
        private static Logger? _instance;
        private readonly string _logDir;
        private ILogFormatter _formatter = new JsonFormatter();
        private LogDestination _destination = LogDestination.Local;
        private RemoteLogFormatter? _remoteFormatter;

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

        public void SetFormatter(ILogFormatter formatter) => _formatter = formatter;

        public void SetDestination(LogDestination destination) => _destination = destination;

        public void SetRemoteFormatter(string serverUrl)
        {
            _remoteFormatter = new RemoteLogFormatter(serverUrl);
        }

        public void Log(LogEntry entry)
        {
            string ext = _formatter.GetExtension();
            string fileName = DateTime.Now.ToString("yyyy-MM-dd") + "." + ext;
            string content = _formatter.Format(entry) + Environment.NewLine;

            if (_destination == LogDestination.Local || _destination == LogDestination.Both)
                File.AppendAllText(Path.Combine(_logDir, fileName), content);

            if (_destination == LogDestination.Remote || _destination == LogDestination.Both)
                if (_remoteFormatter != null)
                    _ = _remoteFormatter.SendAsync(entry);
        }

        public string GetLogPath() => _logDir;
    }

    public enum LogDestination { Local, Remote, Both }
}
