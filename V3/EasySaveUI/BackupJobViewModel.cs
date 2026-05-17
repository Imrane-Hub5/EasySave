using System.ComponentModel;
using System.Windows.Input;
using EasySave.Models;
using EasySave.Services;

namespace EasySaveUI
{
    /// <summary>
    /// ViewModel for a single backup job.
    /// Implements INotifyPropertyChanged for real-time UI updates.
    /// Exposes Pause, Play and Stop commands bound to BackupJobController.
    /// </summary>
    public class BackupJobViewModel : INotifyPropertyChanged
    {
        private readonly BackupJob _job;
        private readonly BackupJobController _controller;
        private string _status = "Inactive";
        private double _progression = 0;

        public BackupJobViewModel(BackupJob job)
        {
            _job = job;
            _controller = job.Controller;

            PauseCommand = new RelayCommand(() => _controller.Pause());
            PlayCommand = new RelayCommand(() => _controller.Play());
            StopCommand = new RelayCommand(() => _controller.Stop());
        }

        /// <summary>Name of the backup job.</summary>
        public string Name => _job.Name;

        /// <summary>Source directory path.</summary>
        public string SourcePath => _job.SourcePath;

        /// <summary>Target directory path.</summary>
        public string TargetPath => _job.TargetPath;

        /// <summary>Backup type (Full or Differential).</summary>
        public string Type => _job.Type.ToString();

        /// <summary>Current status of the job (Inactive, Active, Paused, Stopped).</summary>
        public string Status
        {
            get => _status;
            set
            {
                _status = value;
                OnPropertyChanged(nameof(Status));
            }
        }

        /// <summary>Progression percentage from 0 to 100.</summary>
        public double Progression
        {
            get => _progression;
            set
            {
                _progression = value;
                OnPropertyChanged(nameof(Progression));
            }
        }

        /// <summary>Pauses the backup job after current file transfer completes.</summary>
        public ICommand PauseCommand { get; }

        /// <summary>Resumes the backup job from a paused state.</summary>
        public ICommand PlayCommand { get; }

        /// <summary>Stops the backup job immediately.</summary>
        public ICommand StopCommand { get; }

        /// <inheritdoc/>
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>Notifies the UI that a property value has changed.</summary>
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}