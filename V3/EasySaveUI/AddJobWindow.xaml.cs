using System.Windows;
using EasySave.Models;
using Microsoft.Win32;

namespace EasySaveUI
{
    public partial class AddJobWindow : Window
    {
        public BackupJob? NewJob { get; private set; }

        public AddJobWindow()
        {
            InitializeComponent();
        }

        private void BtnBrowseSource_Click(object sender, RoutedEventArgs e)
        {
            OpenFolderDialog dialog = new OpenFolderDialog();
            if (dialog.ShowDialog() == true)
                TxtSource.Text = dialog.FolderName;
        }

        private void BtnBrowseTarget_Click(object sender, RoutedEventArgs e)
        {
            OpenFolderDialog dialog = new OpenFolderDialog();
            if (dialog.ShowDialog() == true)
                TxtTarget.Text = dialog.FolderName;
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TxtName.Text) ||
                string.IsNullOrWhiteSpace(TxtSource.Text) ||
                string.IsNullOrWhiteSpace(TxtTarget.Text))
            {
                MessageBox.Show("Please fill in all fields.");
                return;
            }

            BackupType type = CmbType.SelectedIndex == 0
                ? BackupType.Complete
                : BackupType.Differential;

            NewJob = new BackupJob
            {
                Name = TxtName.Text,
                SourcePath = TxtSource.Text,
                TargetPath = TxtTarget.Text,
                Type = type
            };

            DialogResult = true;
            Close();
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}