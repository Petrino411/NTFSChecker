using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NTFSChecker.Avalonia.DTO;
using NTFSChecker.Avalonia.Services;

namespace NTFSChecker.Avalonia;

public partial class MainWindow : Window
{
    private readonly ExcelWriter _excelWriter;
    private readonly ILogger<MainWindow> _logger;

    private readonly Stopwatch _stopwatch;
    private string SelectedFolderPath { get; set; }

    private readonly DirectoryChecker _directoryChecker;
    private readonly UserGroupHelper _reGroupHelper;
    private readonly SettingsForm _settingsForm;

    private int _files;
    private int _directories;

    public event EventHandler<int> ItemsCounted;


    public MainWindow(ExcelWriter excelWriter, DirectoryChecker directoryChecker, UserGroupHelper reGroupHelper,
        ILogger<MainWindow> logger, SettingsForm settingsForm)
    {
        _excelWriter = excelWriter;
        _directoryChecker = directoryChecker;
        _reGroupHelper = reGroupHelper;
        _logger = logger;
        _settingsForm = settingsForm;
        InitializeComponent();
    }

    private void ConfigureEvents()
    {
        _directoryChecker.LogMessage += OnLogMessage;
        _directoryChecker.ProgressUpdate += OnProgressUpdate;
    }

    private async void BtnOpen_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            var topLevel = GetTopLevel(this); 

            if (topLevel == null) return;

            var options = new FolderPickerOpenOptions
            {
                Title = "Выберите папку",
                SuggestedStartLocation = await topLevel.StorageProvider.TryGetWellKnownFolderAsync(WellKnownFolder.Documents),
                AllowMultiple = false
            };

            var folders = await topLevel.StorageProvider.OpenFolderPickerAsync(options);

            if (folders.Count <= 0) return;
            var selectedFolderPath = folders[0].Path.LocalPath;
            SelectedFolderPath = selectedFolderPath;
            txtFolderPath.Text = SelectedFolderPath;
            LogToUI($"Выбрана папка: {SelectedFolderPath}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
        }
    }

    private async void BtnCheck_Click(object sender, RoutedEventArgs e)
    {
        StopWatchReset();
        StopWatchStart();
        Reset(resetLogs: false);
        // DisableControls();

        if (string.IsNullOrEmpty(txtFolderPath.Text)) return;
        try
        {
            Task.Run(async () => { await CountItems(txtFolderPath.Text); });
            await Task.Run(async () => { await _directoryChecker.CheckDirectoryAsync(txtFolderPath.Text); });
            LogToUI("Операция прошла успешно");
            progressBar.Value = progressBar.Maximum;
            StopWatchStop();
            // EnableControls();
        }
        catch (Exception exception)
        {
            _logger.LogError(exception.ToString());
            throw;
        }
    }

    private async Task CountItems(string path)
    {
        var totalItems = Directory.GetFiles(path, "*", SearchOption.AllDirectories).Length +
                         Directory.GetDirectories(path, "*", SearchOption.AllDirectories).Length;
        ItemsCounted?.Invoke(this, totalItems);
    }

    private void Reset(bool resetLogs = true)
    {
        progressBar.Value = 0;
        progressBar.Maximum = 100;
        _directoryChecker.RootData.Clear();
        if (resetLogs)
        {
            ListLogs.Items.Clear();
        }

        labelInfo.Text = $"Проверено:\nпапок:\nфайлов:";
    }

    // private void OnItemsCounted(object sender, int count)
    // {
    //     Invoke(new Action(() => progressBar.Maximum = count+1));
    //     Invoke(new Action(() => progressBar.Value = _files + _directories));
    // }


    private void ExportToExcelClick(object sender, RoutedEventArgs e)
    {
        Task.Run(async () =>
        {
            try
            {
                // Invoke(new Action(() => Cursor = Cursors.WaitCursor));
                //
                // if (_directoryChecker.RootData.Count == 0)
                // {
                //     Invoke(new Action(() => Cursor = Cursors.Default));
                //     return;
                // }


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


                var data = await PrepareDataToExport();
                _excelWriter.CreateNewFile();
                await _excelWriter.SetTableHeadAsync(headers);
                await _excelWriter.WriteDataAsync(data);
                await _excelWriter.CreateLegendAsync();
                await _excelWriter.AutoFitColumnsAndRowsAsync();

                await _excelWriter.SaveTempAndShowAsync();
                // Invoke(new Action(() => Cursor = Cursors.Default));
            }
            catch (Exception ex)
            {
                // MessageBox.Show($"An error occurred: {ex.Message}");
                _logger.LogError(ex.ToString());
            }
        });
    }

    private async Task<List<ExcelDataModel>> PrepareDataToExport()
    {
        List<ExcelDataModel> data = [];
        bool.TryParse(App.Configuration["AppSettings:ExportAll"], out bool allExp);
        _reGroupHelper.CheckDomainAvailability();
        if (!allExp)
        {
            data.Add(_directoryChecker.RootData.FirstOrDefault());
            data.AddRange(_directoryChecker.RootData.Where(x => x.ChangesFlag).ToList());
        }
        else
        {
            data = _directoryChecker.RootData;
        }

        return await _reGroupHelper.SetDescriptionsAsync(data);
    }

    private void OnProgressUpdate(object sender, (int dirs, int files) info)
    {
        _files = info.files;
        _directories = info.dirs;
        var max = 0;
        // // Invoke(new Action(() => max = progressBar.Maximum));
        // if (max > 100)
        // {
        //     Invoke(new Action(() => progressBar.Value = _files + _directories));
        // }
        //
        //
        // Invoke(new Action(() => labelInfo.Text = $"Проверено:\nпапок:{info.dirs}\nфайлов:{info.files}"));
    }


    private void OnLogMessage(object sender, string message)
    {
        LogToUI(message);
    }

    private void LogToUI(string message)
    {
        // if (InvokeRequired)
        // {
        //     Invoke(new Action(() => ListLogs.Items.Add(message)));
        //     Invoke(new Action(() => ListLogs.SelectedItem = message));
        // }
        // else
        {
            ListLogs.Items.Add(message);
            ListLogs.SelectedItem = message;
        }

        _logger.LogInformation(message);
    }


    private void StopWatchStart()
    {
        // Timer1.Start();
        _stopwatch.Start();
    }

    private void StopWatchStop()
    {
        // Timer1.Stop();
        _stopwatch.Stop();
    }

    private void StopWatchReset()
    {
        _stopwatch.Reset();
        labelTimer.Text = "00:00:00:000";
    }

    private void TimerTick(object sender, RoutedEventArgs e)
    {
        var elapsed = _stopwatch.Elapsed;
        labelTimer.Text =
            $"{Math.Floor(elapsed.TotalHours):00}:{elapsed.Minutes:00}:{elapsed.Seconds:00}:{elapsed.Milliseconds:00}";
    }

    private void DisableControls()
    {
        BtnOpen.IsEnabled = false;
        BtnCheck.IsEnabled = false;
        txtFolderPath.IsEnabled = false;
        MenuDock.IsEnabled = false;
    }
    
    private void EnableControls()
    {
        BtnOpen.IsEnabled = true;
        BtnCheck.IsEnabled = true;
        txtFolderPath.IsEnabled = true;
        MenuDock.IsEnabled = true;
    }

    private void SettingsClick(object? sender, RoutedEventArgs e)
    {
        _settingsForm.Show();
    }


}