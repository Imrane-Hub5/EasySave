using System.Windows;
using EasySave.Models;
using EasySave.Services;

namespace EasySaveUI
{
    public partial class MainWindow : Window
    {
        private readonly BackupManager _backupManager;

        public MainWindow()
        {
            InitializeComponent();
            _backupManager = BackupManager.GetInstance();
            RefreshJobList();
        }

        /// <summary>
        /// Refreshes the job list displayed in the DataGrid
        /// </summary>
        private void RefreshJobList()
        {
            JobsGrid.ItemsSource = null;
            JobsGrid.ItemsSource = _backupManager.Jobs;
        }

        /// <summary>
        /// Opens the Add Job dialog
        /// </summary>
        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            AddJobWindow addWindow = new AddJobWindow();
            addWindow.Owner = this;
            if (addWindow.ShowDialog() == true)
            {
                _backupManager.AddJob(addWindow.NewJob);
                RefreshJobList();
            }
        }

        /// <summary>
        /// Removes the selected job
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
        /// Runs the selected job
        /// </summary>
        private void BtnRun_Click(object sender, RoutedEventArgs e)
        {
            int index = JobsGrid.SelectedIndex;
            if (index < 0)
            {
                MessageBox.Show("Please select a job to run.");
                return;
            }
            _backupManager.RunJob(index);
            MessageBox.Show("Backup completed!");
        }

        /// <summary>
        /// Runs all jobs
        /// </summary>
        private void BtnRunAll_Click(object sender, RoutedEventArgs e)
        {
            _backupManager.RunAll();
            MessageBox.Show("All backups completed!");
        }

        /// <summary>
        /// Opens the Settings window
        /// </summary>
        private void BtnSettings_Click(object sender, RoutedEventArgs e)
        {
            SettingsWindow settingsWindow = new SettingsWindow();
            settingsWindow.Owner = this;
            settingsWindow.ShowDialog();
        }
    }
}