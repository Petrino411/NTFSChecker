using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Platform.Storage;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using NTFSChecker.AvaloniaUI.Interfaces;
using NTFSChecker.AvaloniaUI.Services;
using NTFSChecker.AvaloniaUI.Views;
using NTFSChecker.Core.Interfaces;
using NTFSChecker.Core.Models;
using NTFSChecker.Core.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Win32;

namespace NTFSChecker.AvaloniaUI.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    private readonly IDirectoryChecker _directoryChecker;
    private readonly IExcelWriter _excelWriter;
    private readonly IUserGroupHelper _reGroupHelper;
    private readonly ILogger<MainWindowViewModel> _logger;
    private readonly ISettingsService _settingsService;
    private readonly IWindowService _windowService;

    public ObservableCollection<string> Logs { get; } = new();

    [ObservableProperty] private string selectedFolderPath;
    [ObservableProperty] private double progressValue;
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
        IDirectoryChecker directoryChecker,
        IExcelWriter excelWriter,
        IUserGroupHelper reGroupHelper,
        ILogger<MainWindowViewModel> logger, 
        ISettingsService settingsService,
        IWindowService windowService)
    {
        _directoryChecker = directoryChecker;
        _excelWriter = excelWriter;
        _reGroupHelper = reGroupHelper;
        _logger = logger;
        _settingsService = settingsService;
        _windowService = windowService;
        ProgressValue = 0;
        ProgressMax = 100;
    }


    [RelayCommand]
    private async Task SelectFolderAsync(Window window)
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            var dialog = new OpenFolderDialog
            {
                Title = "Выберите папку",
            };

            var result = await Task.Run(() => dialog.ShowAsync(window));

            if (!string.IsNullOrEmpty(result))
            {
                SelectedFolderPath = result;
                Logs.Add($"Выбрана папка: {SelectedFolderPath}");
            }
        }
        else
        {
            var topLevel = TopLevel.GetTopLevel(window);
            if (topLevel?.StorageProvider is { CanOpen: true } storageProvider)
            {
                var folders = await storageProvider.OpenFolderPickerAsync(new FolderPickerOpenOptions
                {
                    Title = "Выберите папку",
                    AllowMultiple = false
                });

                if (folders.Count > 0)
                {
                    SelectedFolderPath = folders[0].Path.LocalPath;
                    Logs.Add($"Выбрана папка: {SelectedFolderPath}");
                }
            }
            else
            {
                Logs.Add("Платформа не поддерживает выбор папки.");
            }
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

            await _excelWriter.CreateNewFile(SelectedFolderPath, headers);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при экспорте в Excel");
        }

    }

    [RelayCommand]
    public async Task SettingsShow()
    {
        _windowService.ShowWindow<SettingsForm, SettingsWinViewModel>();
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