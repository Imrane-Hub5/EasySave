using System;
using System.Windows;
using EasySave.Models;
using EasySave.Services;
using EasyLog;

namespace EasySaveUI
{
    /// <summary>
    /// Entry point — launches WPF application or runs backup from command line
    /// </summary>
    public class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            // Load settings
            Settings settings = Settings.Load();

            // Set log formatter based on settings
            ILogFormatter formatter = settings.LogFormat == "XML"
                ? new XmlFormatter()
                : new JsonFormatter();
            Logger.GetInstance().SetFormatter(formatter);

            // If launched with args (ex: EasySave.exe 1-3)
            if (args.Length > 0)
            {
                BackupManager.GetInstance().ExecuteRange(args[0]);
                return;
            }

            // Launch WPF
            Application app = new Application();
            MainWindow window = new MainWindow();
            app.Run(window);
        }
    }
}