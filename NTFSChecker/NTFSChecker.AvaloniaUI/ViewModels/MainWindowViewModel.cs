using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NTFSChecker.AvaloniaUI.Interfaces;
using NTFSChecker.AvaloniaUI.Models;
using NTFSChecker.AvaloniaUI.Views;
using NTFSChecker.Core.Interfaces;

namespace NTFSChecker.AvaloniaUI.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    private readonly IExcelWriter _excelWriter;
    private readonly ILogger<MainWindowViewModel> _logger;
    private readonly IWindowService _windowService;
    [ObservableProperty] private ViewModelBase currentPage;
    [ObservableProperty] private bool isMenuVisible = true;
    [ObservableProperty] private bool isPaneOpen;
    [ObservableProperty] private ListItemTemplate selectedListItem;


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
            new(typeof(MainPageViewModel), "HomeRegular", "Домашняя страница"),
            new(typeof(StatisticsPageViewModel), "DataUsageRegular", "Статистика")
        };

        SelectedListItem = Items.First();

        _excelWriter = excelWriter;
        _logger = logger;
        _windowService = windowService;

        CurrentPage = App.Services.GetRequiredService<MainPageViewModel>();
    }

    public ObservableCollection<ListItemTemplate> Items { get; }

    partial void OnSelectedListItemChanged(ListItemTemplate? value)
    {
        if (value is null) return;

        var vm = Design.IsDesignMode
            ? Activator.CreateInstance(value.ModelType)
            : App.Services.GetService(value.ModelType);


        if (vm is not ViewModelBase vmb) return;

        CurrentPage = vmb;
        if (vmb is StatisticsPageViewModel) App.Services.GetService<StatisticsPageViewModel>()?.RefreshIfNeeded();
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

            await _excelWriter.CreateNewFile(App.Services.GetRequiredService<MainPageViewModel>().SelectedFolderPath,
                headers);
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