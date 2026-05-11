using System.ComponentModel;
using EasySave.Models;

namespace EasySaveUI
{
    /// <summary>
    /// ViewModel for a single backup job
    /// Updates UI in real time via INotifyPropertyChanged
    /// </summary>
    public class BackupJobViewModel : INotifyPropertyChanged
    {
        private BackupJob _job;
        private string _status = "Inactive";
        private double _progression = 0;

        public BackupJobViewModel(BackupJob job)
        {
            _job = job;
        }

        // Name of the backup job
        public string Name => _job.Name;

        // Source path
        public string SourcePath => _job.SourcePath;

        // Target path
        public string TargetPath => _job.TargetPath;

        // Type (Full or Differential)
        public string Type => _job.Type;

        // Status (Active / Inactive)
        public string Status
        {
            get => _status;
            set
            {
                _status = value;
                OnPropertyChanged(nameof(Status));
            }
        }

        // Progression 0 to 100
        public double Progression
        {
            get => _progression;
            set
            {
                _progression = value;
                OnPropertyChanged(nameof(Progression));
            }
        }

        // INotifyPropertyChanged
        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}