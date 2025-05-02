using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;
using Avalonia.Threading;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NTFSChecker.Avalonia.DTO;
using NTFSChecker.Avalonia.Services;

namespace NTFSChecker.Avalonia;

public partial class MainWindow : Window
{
    private readonly ExcelWriter _excelWriter;
    private readonly ILogger<MainWindow> _logger;
    private string SelectedFolderPath { get; set; }

    private readonly DirectoryChecker _directoryChecker;
    private readonly UserGroupHelper _reGroupHelper;
    
    private Stopwatch _stopwatch = new();
    private CancellationTokenSource _timerCts;


    private int _files;
    private int _directories;

    public Action<int> ItemsCounted;

    public MainWindow()
    {
        InitializeComponent();
    }

    public MainWindow(ExcelWriter excelWriter, DirectoryChecker directoryChecker, UserGroupHelper reGroupHelper,
        ILogger<MainWindow> logger)
    {
        InitializeComponent();
        _excelWriter = excelWriter;
        _directoryChecker = directoryChecker;
        _reGroupHelper = reGroupHelper;
        _logger = logger;
        ConfigureEvents();
    }

    private void ConfigureEvents()
    {
        _directoryChecker.logAction = OnLogMessage;
        _directoryChecker.progressAction = OnProgressUpdate;
        ItemsCounted += OnItemsCounted;
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
                SuggestedStartLocation =
                    await topLevel.StorageProvider.TryGetWellKnownFolderAsync(WellKnownFolder.Documents),
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
        try
        {
            if (string.IsNullOrEmpty(txtFolderPath.Text)) return;
            DisableControls();
            Reset();
            CountItems(txtFolderPath.Text);
            
            _stopwatch.Restart();
            _timerCts = new CancellationTokenSource();
            _ = UpdateTimerAsync(_timerCts.Token);
            
            await _directoryChecker.CheckDirectoryAsync(txtFolderPath.Text);
            await Dispatcher.UIThread.InvokeAsync(() =>
            {
                LogToUI("Операция прошла успешно");
                progressBar.Value = progressBar.Maximum;
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());
            await Dispatcher.UIThread.InvokeAsync(() => { LogToUI($"Ошибка: {ex.Message}"); });
        }
        finally
        {
            EnableControls();
        }
    }
    private async Task UpdateTimerAsync(CancellationToken token)
    {
        try
        {
            while (!token.IsCancellationRequested)
            {
                var elapsed = _stopwatch.Elapsed;
                var formatted = $"{elapsed:hh\\:mm\\:ss\\.fff}";
                await Dispatcher.UIThread.InvokeAsync(() => labelTimer.Text = formatted);
                await Task.Delay(100, token);
            }
        }
        catch (TaskCanceledException)
        {
        }
    }

    private async Task CountItems(string path)
    {
        var totalItems = Directory.GetFiles(path, "*", SearchOption.AllDirectories).Length +
                         Directory.GetDirectories(path, "*", SearchOption.AllDirectories).Length;
        await Dispatcher.UIThread.InvokeAsync(() => { ItemsCounted(totalItems); });
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

        labelInfo.Text = $"Проверено:";
    }

    private void ExportToExcelClick(object sender, RoutedEventArgs e)
    {
        Task.Run(async () =>
        {
            try
            {
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

    private void OnProgressUpdate((int dirs, int files) info)
    {
        _files = info.files;
        _directories = info.dirs;
        double max = 0;
        Dispatcher.UIThread.Invoke(() => max = progressBar.Maximum);
        if (max > 100)
        {
            Dispatcher.UIThread.InvokeAsync(() => progressBar.Value = _files + _directories);
        }


        Dispatcher.UIThread.InvokeAsync(() => labelInfo.Text = $"Проверено:\nпапок:{info.dirs}\nфайлов:{info.files}");
    }


    private void OnLogMessage(string message)
    {
        LogToUI(message);
    }

    private void LogToUI(string message)
    {
        Dispatcher.UIThread.InvokeAsync(() => ListLogs.Items.Add(message));
        Dispatcher.UIThread.InvokeAsync(() => ListLogs.SelectedItem = message);

        _logger.LogInformation(message);
    }

    private void OnItemsCounted(int count)
    {
        Dispatcher.UIThread.InvokeAsync(() => progressBar.Maximum = count + 1);
        Dispatcher.UIThread.InvokeAsync(() => progressBar.Value = _files + _directories);
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
        var settingsForm = App.ServiceProvider.GetRequiredService<SettingsForm>();
        settingsForm.Show();
    }
}