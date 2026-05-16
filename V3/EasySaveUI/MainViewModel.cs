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
                // Load settings to get the configured business software name
                Settings settings = Settings.Load();
                string softwareName = settings.BusinessSoftware;

                while (true)
                {
                    // Update the global static flag inside BusinessSoftwareService
                    _softwareService.MonitorProcess(softwareName);

                    // Wait 1 second before checking again
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
