using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using EasySave.Services;

namespace EasySaveUI
{
    /// <summary>
    /// ViewModel for the Settings window — binds Settings model to SettingsWindow
    /// </summary>
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
            set
            {
                _settings.LogFormat = value;
                OnPropertyChanged();
            }
        }

        public string BusinessSoftware
        {
            get => _settings.BusinessSoftware;
            set
            {
                _settings.BusinessSoftware = value;
                OnPropertyChanged();
            }
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

        public ICommand SaveCommand { get; }

        private void Save()
        {
            _settings.Save();
        }

        private void OnPropertyChanged([CallerMemberName] string? name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
