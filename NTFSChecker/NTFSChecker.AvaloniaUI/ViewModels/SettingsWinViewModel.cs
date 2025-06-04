using System;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using NTFSChecker.Core.Interfaces;
using NTFSChecker.Core.Models;

namespace NTFSChecker.AvaloniaUI.ViewModels;

public partial class SettingsWinViewModel : ViewModelBase
{
    private readonly ILogger<SettingsWinViewModel> _logger;
    private readonly ISettingsService _settingsService;

    [ObservableProperty] private AppSettings settings = new();

    public SettingsWinViewModel()
    {
    }

    public SettingsWinViewModel(ILogger<SettingsWinViewModel> logger, ISettingsService settingsService)
    {
        _logger = logger;
        _settingsService = settingsService;
        Settings = _settingsService.Load();
    }

    public Action? CloseAction { get; set; }


    [RelayCommand]
    private void Save()
    {
        _settingsService.Save(Settings);
        CloseAction?.Invoke();
    }
}