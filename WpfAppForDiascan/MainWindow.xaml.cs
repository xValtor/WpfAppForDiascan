using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Win32;
using System.Threading;

namespace WpfAppForDiascan
{
    public partial class MainWindow : Window
    {
        private readonly string _connectionString = "Host=localhost;Username=postgres;Password=9642;Database=hashdb";

        public MainWindow()
        {
            InitializeComponent();
        }

        private async void Select_File(object sender, RoutedEventArgs e)
        {
            var dlg = new OpenFileDialog
            {
                Multiselect = true,
                Title = "Выберите файлы для расчёта хеш-суммы"
            };

            if (dlg.ShowDialog() == true)
            {
                TxtLog.Text = "";
                List<string> files = new List<string>(dlg.FileNames);
                await CalculateAndSaveHashe(files);
            }
        }
        private Task CalculateAndSaveHashe(List<string> files)
        {
            return Task.Run(() =>
            {
                using (var db = new DatabaseHelper(_connectionString))
                {
                    foreach (var file in files)
                    {
                        string hash = HashCalculator.Calculate(file);
                        db.SaveFileHash(file, hash);
                        Dispatcher.Invoke(() =>
                        {
                            TxtLog.AppendText($"Файл: {file}\nХеш: {hash}\n\n");
                        });
                        Thread.Sleep(100);
                    }
                }
            });
        }
    }
}
