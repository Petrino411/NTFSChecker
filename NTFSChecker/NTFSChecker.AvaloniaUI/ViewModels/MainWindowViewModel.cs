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
using System.Windows.Input;
using CommunityToolkit.Mvvm.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Win32;
using NTFSChecker.AvaloniaUI.Models;

namespace NTFSChecker.AvaloniaUI.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    private readonly IExcelWriter _excelWriter;
    private readonly ILogger<MainWindowViewModel> _logger;
    private readonly IWindowService _windowService;

    public ObservableCollection<ListItemTemplate> Items { get; }
    [ObservableProperty] private ListItemTemplate selectedListItem;
    [ObservableProperty] private ViewModelBase currentPage;
    [ObservableProperty] private bool isPaneOpen = true;
    [ObservableProperty] private bool isMenuVisible = true;
    
    
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
        Items = new ObservableCollection<ListItemTemplate>
        {
            new(typeof(MainPageViewModel), "HomeRegular", "Домашняя страница" ),
            new(typeof(StatisticsPageViewModel), "DataUsageRegular", "Статистика")
        };

        SelectedListItem = Items.First();
        
        _excelWriter = excelWriter;
        _logger = logger;
        _windowService = windowService;
        
        CurrentPage = App.Services.GetRequiredService<MainPageViewModel>();
        
        
    }
    
    partial void OnSelectedListItemChanged(ListItemTemplate? value)
    {
        if (value is null) return;

        var vm = Design.IsDesignMode
            ? Activator.CreateInstance(value.ModelType)
            : App.Services.GetService(value.ModelType);
        

        if (vm is not ViewModelBase vmb) return;

        CurrentPage = vmb;
        if (vmb is StatisticsPageViewModel)
        {
            App.Services.GetService<StatisticsPageViewModel>()?.RefreshIfNeeded();
        }

    }


    [RelayCommand]
    public void TriggerPane()
    {
        IsPaneOpen = !IsPaneOpen;
    }

    [RelayCommand]
    public void ToggleMenu()
    {
        IsMenuVisible = !IsMenuVisible;
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

            await _excelWriter.CreateNewFile(App.Services.GetRequiredService<MainPageViewModel>().SelectedFolderPath, headers);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при экспорте в Excel");
        }
    }

    [RelayCommand]
    public void SettingsShow()
    {
        _windowService.ShowWindow<SettingsForm, SettingsWinViewModel>();
    }


   
}