using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using EasySave.Services;

namespace EasySaveUI
{
    public class SettingsViewModel : INotifyPropertyChanged
    {
        private readonly Settings _settings;

        public event PropertyChangedEventHandler? PropertyChanged;

        public SettingsViewModel()
        {
            _settings = Settings.Load();
            SaveCommand = new RelayCommand(Save);
        }

        public string LogFormat
        {
            get => _settings.LogFormat;
            set { _settings.LogFormat = value; OnPropertyChanged(); }
        }

        public string BusinessSoftware
        {
            get => _settings.BusinessSoftware;
            set { _settings.BusinessSoftware = value; OnPropertyChanged(); }
        }

        public string EncryptedExtensions
        {
            get => string.Join(", ", _settings.EncryptedExtensions);
            set
            {
                _settings.EncryptedExtensions = new List<string>(
                    value.Split(',', System.StringSplitOptions.RemoveEmptyEntries | System.StringSplitOptions.TrimEntries)
                );
                OnPropertyChanged();
            }
        }

        public string PriorityExtensions
        {
            get => string.Join(", ", _settings.PriorityExtensions);
            set
            {
                _settings.PriorityExtensions = new List<string>(
                    value.Split(',', System.StringSplitOptions.RemoveEmptyEntries | System.StringSplitOptions.TrimEntries)
                );
                OnPropertyChanged();
            }
        }

        public string MaxParallelFileSizeKo
        {
            get => _settings.MaxParallelFileSizeKo.ToString();
            set
            {
                if (long.TryParse(value, out long v) && v > 0)
                {
                    _settings.MaxParallelFileSizeKo = v;
                    OnPropertyChanged();
                }
            }
        }

        public string CryptoSoftPath
        {
            get => _settings.CryptoSoftPath;
            set { _settings.CryptoSoftPath = value; OnPropertyChanged(); }
        }

        public string DockerServerUrl
        {
            get => _settings.DockerServerUrl;
            set { _settings.DockerServerUrl = value; OnPropertyChanged(); }
        }

        public bool IsLocalOnly
        {
            get => _settings.LogDestination == LogDestination.Local;
            set { if (value) { _settings.LogDestination = LogDestination.Local; OnPropertyChanged(); } }
        }

        public bool IsRemoteOnly
        {
            get => _settings.LogDestination == LogDestination.Remote;
            set { if (value) { _settings.LogDestination = LogDestination.Remote; OnPropertyChanged(); } }
        }

        public bool IsBoth
        {
            get => _settings.LogDestination == LogDestination.Both;
            set { if (value) { _settings.LogDestination = LogDestination.Both; OnPropertyChanged(); } }
        }

        public ICommand SaveCommand { get; }

        private void Save() => _settings.Save();

        private void OnPropertyChanged([CallerMemberName] string? name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
