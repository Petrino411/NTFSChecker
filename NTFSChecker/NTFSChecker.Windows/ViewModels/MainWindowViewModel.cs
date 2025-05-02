using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Platform.Storage;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NTFSChecker.Windows.Models;
using NTFSChecker.Windows.Services;
using NTFSChecker.Windows.Views;

namespace NTFSChecker.Windows.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    private readonly DirectoryChecker _directoryChecker;
    private readonly ExcelWriter _excelWriter;
    private readonly UserGroupHelper _reGroupHelper;
    private readonly ILogger<MainWindowViewModel> _logger;
    private readonly SettingsService _settingsService;
    private readonly IWindowService _windowService;

    public ObservableCollection<string> Logs { get; } = new();

    [ObservableProperty] private string selectedFolderPath;
    [ObservableProperty] private double progressValue = 0;
    [ObservableProperty] private double progressMax;
    [ObservableProperty] private string labelText;
    [ObservableProperty] private string labelTimer;
    [ObservableProperty] private int? selectedLogInd;


    private int _dirs;
    private int _files;
    private Stopwatch _stopwatch = new();
    private CancellationTokenSource? _cts;

    public MainWindowViewModel()
    {
    }

    public MainWindowViewModel(
        DirectoryChecker directoryChecker,
        ExcelWriter excelWriter,
        UserGroupHelper reGroupHelper,
        ILogger<MainWindowViewModel> logger, 
        SettingsService settingsService,
        IWindowService windowService)
    {
        _directoryChecker = directoryChecker;
        _excelWriter = excelWriter;
        _reGroupHelper = reGroupHelper;
        _logger = logger;
        _settingsService = settingsService;
        _windowService = windowService;
    }


    [RelayCommand]
    private async Task SelectFolderAsync(Window window)
    {
        var topLevel = TopLevel.GetTopLevel(window);
        if (topLevel?.StorageProvider is { CanOpen: true } storageProvider)
        {
            var folders = await storageProvider.OpenFolderPickerAsync(new FolderPickerOpenOptions
            {
                Title = "Выберите папку",
                AllowMultiple = false
            });

            SelectedFolderPath = folders[0].Path.LocalPath;
            Logs.Add($"Выбрана папка: {SelectedFolderPath}");
        }
        else
        {
            Logs.Add("Платформа не поддерживает выбор папки.");
        }
    }

    [RelayCommand]
    public async Task CheckDirectoryAsync()
    {
        if (string.IsNullOrWhiteSpace(SelectedFolderPath))
            return;

        Logs.Clear();
        ProgressValue = 0;
        _dirs = _files = 0;
        CountItems(SelectedFolderPath);
        Logs.Add("Проверка началась...");
        _stopwatch.Restart();
        _cts = new CancellationTokenSource();
        UpdateTimerAsync(_cts.Token);
        try
        {
            await _directoryChecker.CheckDirectoryAsync(SelectedFolderPath,
                LogToUIAsync,
                UpdateProgressAsync);
            Logs.Add("Операция прошла успешно");
        }
        catch (Exception ex)
        {
            Logs.Add($"Ошибка: {ex.Message}");
            _logger.LogError(ex, "Ошибка проверки директории");
        }
        finally
        {
            _stopwatch.Stop();
            _cts?.Cancel();
        }
    }

    [RelayCommand]
    public async Task ExportToExcelAsync()
    {
        Task.Run(action: async void () =>
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

    [RelayCommand]
    public async Task SettingsShow()
    {
        _windowService.ShowWindow<SettingsForm, SettingsWinViewModel>();
    }

    private async Task<List<ExcelDataModel>> PrepareDataToExport()
    {
        List<ExcelDataModel> data = [];
        bool.TryParse(_settingsService.GetString("AppSettings:ExportAll"), out bool allExp);
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

    private async Task CountItems(string path)
    {
        Task.Run(action: async void () =>
        {
            var totalItems = Directory.GetFiles(path, "*", SearchOption.AllDirectories).Length +
                             Directory.GetDirectories(path, "*", SearchOption.AllDirectories).Length;
            await Dispatcher.UIThread.InvokeAsync(() => ProgressMax = totalItems + 1);
            await Dispatcher.UIThread.InvokeAsync(() => ProgressValue = _files + _dirs);
        });
    }

    private async Task UpdateProgressAsync(int dirs, int files)
    {
        _dirs = dirs;
        _files = files;
        await Dispatcher.UIThread.InvokeAsync(() =>
        {
            ProgressValue = _dirs + _files;
            LabelText = $"Проверено: папок: {_dirs}, файлов: {_files}";
        });
    }

    private async Task LogToUIAsync(string message)
    {
        await Dispatcher.UIThread.InvokeAsync(() => { Logs.Add(message); });
        SelectedLogInd = Logs.Count - 1;
        _logger.LogInformation(message); // Можно оставить как есть
    }

    private async Task UpdateTimerAsync(CancellationToken token)
    {
        try
        {
            while (!token.IsCancellationRequested)
            {
                var elapsed = _stopwatch.Elapsed;
                LabelTimer = $"{elapsed:hh\\:mm\\:ss\\.fff}";
                await Task.Delay(100, token);
            }
        }
        catch (TaskCanceledException)
        {
        }
    }
}