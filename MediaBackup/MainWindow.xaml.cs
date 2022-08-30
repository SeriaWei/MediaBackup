using MediaBackup.IO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;
using System.Windows;

namespace MediaBackup
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            ListView_SyncItems.ItemsSource = new ObservableCollection<SyncInfo>();
        }

        private void Button_SourcePath_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Button_BackupPath_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Start_Click(object sender, RoutedEventArgs e)
        {
            ((ObservableCollection<SyncInfo>)ListView_SyncItems.ItemsSource).Clear();
            Button_Start.IsEnabled = false;
            Task.Factory.StartNew(() =>
            {
                var sourceMediaManager = new LocalMediaManager(GetSourcePath());
                var backupMediaManager = new LocalMediaManager(GetBackupPath());

                foreach (var item in sourceMediaManager.GetFiles())
                {
                    using (var fs = sourceMediaManager.GetFileStream(item))
                    {
                        DateTime creationTime = sourceMediaManager.GetCreationTime(item);
                        string mediaType = sourceMediaManager.GetMediaType(item);
                        if (string.IsNullOrEmpty(mediaType))
                        {
                            DispaySyncInfo(new SyncInfo { FilePath = item, Action = "Unknow Media" });
                            continue;
                        }

                        var isSaved = backupMediaManager.SaveFile(fs, Path.Combine(mediaType, creationTime.ToString("yyyyMM")), Path.GetFileName(item));
                        DispaySyncInfo(new SyncInfo { FilePath = item, Action = isSaved ? "Copied" : "Skip" });
                    }
                }
                this.Dispatcher.BeginInvoke(() => { Button_Start.IsEnabled = true; });
            });
        }
        private void DispaySyncInfo(SyncInfo syncItem)
        {
            this.Dispatcher.BeginInvoke(new Action<SyncInfo>(obj =>
            {
                ((ObservableCollection<SyncInfo>)ListView_SyncItems.ItemsSource).Add(obj);
            }), syncItem);
        }
        private string GetSourcePath()
        {
            return this.Dispatcher.Invoke(new Func<string>(() => TextBox_SourcePath.Text));
        }
        private string GetBackupPath()
        {
            return this.Dispatcher.Invoke(new Func<string>(() => TextBox_BackupPath.Text));
        }
    }
}
