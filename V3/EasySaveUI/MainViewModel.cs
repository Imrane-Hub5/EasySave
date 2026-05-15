using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
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
            
            // Unlimited jobs requirement handled by ObservableCollection
            Jobs = new ObservableCollection<BackupJob>(_manager.Jobs);
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
