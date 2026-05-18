using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using EasySave.Models;
using EasySave.Services;

namespace EasySaveUI
{
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

            _job.ProgressChanged += (pct, status) =>
                Application.Current.Dispatcher.Invoke(() =>
                {
                    Progression = pct;
                    Status = status;
                });

            PauseCommand = new RelayCommand(() =>
            {
                _controller.Pause();
                Status = "Paused";
            });

            PlayCommand = new RelayCommand(() =>
            {
                _controller.Play();
                Status = "Active";
            });

            StopCommand = new RelayCommand(() =>
            {
                _controller.Stop();
                Status = "Inactive";
                Progression = 0;
            });
        }

        public string Name       => _job.Name;
        public string SourcePath => _job.SourcePath;
        public string TargetPath => _job.TargetPath;
        public string Type       => _job.Type.ToString();

        public string Status
        {
            get => _status;
            set { _status = value; OnPropertyChanged(nameof(Status)); }
        }

        public double Progression
        {
            get => _progression;
            set { _progression = value; OnPropertyChanged(nameof(Progression)); }
        }

        public ICommand PauseCommand { get; }
        public ICommand PlayCommand  { get; }
        public ICommand StopCommand  { get; }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
