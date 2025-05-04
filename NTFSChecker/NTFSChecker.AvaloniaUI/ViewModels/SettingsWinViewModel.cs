using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using Avalonia.Controls;
using Avalonia.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NTFSChecker.Core.Interfaces;
using NTFSChecker.Core.Models;
using NTFSChecker.Core.Services;

namespace NTFSChecker.AvaloniaUI.ViewModels;

public partial class SettingsWinViewModel : ViewModelBase
{
    private readonly ILogger<SettingsWinViewModel> _logger;
    private readonly ISettingsService _settingsService;
    public Action? CloseAction { get; set; }

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


    [RelayCommand]
    private void Save()
    {
        _settingsService.Save(Settings);
        CloseAction?.Invoke();
    }
}