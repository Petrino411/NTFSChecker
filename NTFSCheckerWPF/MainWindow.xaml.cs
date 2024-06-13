using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Extensions.Logging;
using Microsoft.Win32;
using NTFSChecker.DTO;
using NTFSChecker.Services;
using Path = System.IO.Path;

namespace NTFSChecker
{
    public partial class MainWindow : Window
    {
        private readonly ILogger<MainWindow> _logger;
        private readonly ExcelWriter _excelWriter;
        private string SelectedFolderPath { get; set; }
        private readonly DirectoryChecker _directoryChecker;

        public MainWindow(ILogger<MainWindow> logger, ExcelWriter excelWriter, DirectoryChecker directoryChecker)
        {
            _directoryChecker = directoryChecker;
            _directoryChecker.MainWindow = this;
            _excelWriter = excelWriter;
            _logger = logger;
            InitializeComponent();
        }
        
        private void ScrollViewer_OnScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            if (e.OriginalSource is ScrollViewer scrollViewer &&
                Math.Abs(e.ExtentHeightChange) > 0.0)
            {
                scrollViewer.ScrollToBottom();
            }
        }

        private void OpenBtn_OnClick(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog
            {
                InitialDirectory = @"C:\Users\Petrino\Desktop\PracticeFNS\NTFSCheckerWPF",
                ValidateNames = false,
                CheckFileExists = false,
                CheckPathExists = true,
                FileName = "Выберите"
            };

            if (openFileDialog.ShowDialog() ?? false)
            {
                SelectedFolderPath = Path.GetDirectoryName(openFileDialog.FileName);
                txtFolderPath.Text = SelectedFolderPath;

                txtFolderPath.Text = SelectedFolderPath;
            }
        }

        private async void BtnCheck_Click(object sender, RoutedEventArgs e)
        {
            ProgressBar.Value = 0;
            ListLogs.Items.Clear();
            _directoryChecker.rootData.Clear();

            if (string.IsNullOrEmpty(txtFolderPath.Text)) return;
            
            LogToUI($"Выбрана папка: {SelectedFolderPath}");

            try
            {
                var progress = new Progress<(string message, double progressValue)>(tuple =>
                {

                        LogToUI(tuple.message);
                        ProgressBar.Value = tuple.progressValue * ProgressBar.Maximum;

                });
                var path = txtFolderPath.Text;

                await Task.Run(async () => await _directoryChecker.CheckDirectoryAsync(path, progress));
                
                LogToUI("Операция прошла успешно");

            }
            catch (Exception exception)
            {
                _logger.LogError(exception.ToString());
                MessageBox.Show($"An error occurred: {exception.Message}");
            }
        }

        public void LogToUI(string message)
        {

            // ListLogs.Items.Add(message);

            _logger.LogInformation(message);
        }
        
        

        private async void ExportToExcelClick(object sender, RoutedEventArgs routedEventArgs)
        {
            try
            {
                if (_directoryChecker.rootData.Count == 0) return;

                List<ExcelDataModel> data;
                var headers = new List<string>
                {
                    "Наименование сервера",
                    "IP",
                    "Каталог",
                    "Назначение",
                    "Кому предоставлен доступ",
                    "Назначение прав доступа",
                    "Права доступа",
                    "Тип прав"
                };

                if (ChangesCheckBox.IsChecked == true)
                {
                    data = _directoryChecker.rootData.Where(x => x.ChangesFlag).ToList();
                }
                else
                {
                    data = _directoryChecker.rootData;
                }

                _excelWriter.CreateNewFile();
                await _excelWriter.SetTableHeadAsync(headers);
                await _excelWriter.WriteDataAsync(data);
                await _excelWriter.CreateLegendAsync();
                await _excelWriter.AutoFitColumnsAndRowsAsync();
                await _excelWriter.SaveTempAndShowAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
                _logger.LogError(ex.ToString());
            }
        }
    }
}
