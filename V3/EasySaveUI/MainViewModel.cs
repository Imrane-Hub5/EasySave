using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using EasySave.Models;
using EasySave.Services;

namespace EasySave.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<BackupJob> _jobs = null!;
        private readonly BackupManager _manager;
        private readonly BusinessSoftwareService _softwareService;

        public ObservableCollection<BackupJob> Jobs
        {
            get => _jobs;
            set { _jobs = value; OnPropertyChanged(); }
        }

        public MainViewModel()
        {
            _manager = BackupManager.GetInstance();
            _softwareService = new BusinessSoftwareService();
            
            Jobs = new ObservableCollection<BackupJob>(_manager.Jobs);

            // Start the background monitoring thread/task immediately
            StartMonitoringThread();
        }

        /// <summary>
        /// Monitors the business software process in the background every second
        /// </summary>
        private void StartMonitoringThread()
        {
            Task.Run(async () =>
            {
                while (true)
                {
                    // Reload settings each tick so changes take effect without restart
                    Settings settings = Settings.Load();
                    _softwareService.MonitorProcess(settings.BusinessSoftware);

                    await Task.Delay(1000);
                }
            });
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
