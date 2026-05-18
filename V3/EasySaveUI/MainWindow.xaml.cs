using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Media;
using EasySave.Models;
using EasySave.Services;

namespace EasySaveUI
{
    /// <summary>
    /// Main window — displays backup jobs with real-time progression
    /// and Pause, Play, Stop controls per job row.
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly BackupManager _backupManager;
        private readonly ObservableCollection<BackupJobViewModel> _jobViewModels = new();

        public MainWindow()
        {
            InitializeComponent();
            _backupManager = BackupManager.GetInstance();
            RefreshJobList();
        }

        /// <summary>
        /// Refreshes the job list — wraps each job in a BackupJobViewModel
        /// to expose Pause, Play, Stop commands and real-time progression.
        /// </summary>
        private void RefreshJobList()
        {
            _jobViewModels.Clear();
            foreach (BackupJob job in _backupManager.Jobs)
                _jobViewModels.Add(new BackupJobViewModel(job));

            JobsGrid.ItemsSource = _jobViewModels;
        }

        /// <summary>
        /// Opens the Add Job dialog.
        /// </summary>
        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            AddJobWindow addWindow = new AddJobWindow();
            addWindow.Owner = this;
            if (addWindow.ShowDialog() == true)
            {
                _backupManager.AddJob(addWindow.NewJob!);
                RefreshJobList();
            }
        }

        /// <summary>
        /// Removes the selected job.
        /// </summary>
        private void BtnRemove_Click(object sender, RoutedEventArgs e)
        {
            int index = JobsGrid.SelectedIndex;
            if (index < 0)
            {
                MessageBox.Show("Please select a job to remove.");
                return;
            }
            _backupManager.RemoveJob(index);
            RefreshJobList();
        }

        /// <summary>
        /// Runs the selected job.
        /// </summary>
        private async void BtnRun_Click(object sender, RoutedEventArgs e)
        {
            int index = JobsGrid.SelectedIndex;
            if (index < 0)
            {
                MessageBox.Show("Please select a job to run.");
                return;
            }
            await Task.Run(() => _backupManager.RunJob(index));
            MessageBox.Show("Backup completed!");
        }

        /// <summary>
        /// Runs all jobs in parallel.
        /// </summary>
        private async void BtnRunAll_Click(object sender, RoutedEventArgs e)
        {
            await Task.Run(() => _backupManager.RunAll());
            MessageBox.Show("All backups completed!");
        }

        private void BtnTheme_Click(object sender, RoutedEventArgs e)
        {
            ThemeManager.Toggle();
            if (ThemeManager.IsDark)
            {
                BtnTheme.Content = "☀  Light";
                BtnTheme.BorderBrush = new SolidColorBrush(Color.FromRgb(0xA9, 0xE1, 0x4D));
                BtnTheme.Foreground  = new SolidColorBrush(Color.FromRgb(0xA9, 0xE1, 0x4D));
            }
            else
            {
                BtnTheme.Content = "🌙  Dark";
                BtnTheme.BorderBrush = new SolidColorBrush(Color.FromRgb(0x90, 0xCA, 0xF9));
                BtnTheme.Foreground  = Brushes.White;
            }
        }

        /// <summary>
        /// Opens the Settings window.
        /// </summary>
        private void BtnSettings_Click(object sender, RoutedEventArgs e)
        {
            SettingsWindow settingsWindow = new SettingsWindow();
            settingsWindow.Owner = this;
            settingsWindow.ShowDialog();
        }
    }
}