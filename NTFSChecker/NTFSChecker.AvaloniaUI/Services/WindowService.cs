using System;
using System.Collections.Generic;
using Avalonia.Controls;
using Microsoft.Extensions.DependencyInjection;
using NTFSChecker.AvaloniaUI.Interfaces;

namespace NTFSChecker.AvaloniaUI.Services;

public class WindowService : IWindowService
{
    private readonly IServiceProvider _services;
    private readonly Dictionary<Type, Window> _openWindows = new();

    public WindowService(IServiceProvider services)
    {
        _services = services;
    }

    public void ShowWindow<TWindow, TViewModel>()
        where TWindow : Window
        where TViewModel : class
    {
        var windowType = typeof(TWindow);

        if (_openWindows.TryGetValue(windowType, out var existingWindow) && existingWindow.IsVisible)
        {
            existingWindow.Activate();
            return;
        }

        var viewModel = _services.GetRequiredService<TViewModel>();
        var window = ActivatorUtilities.CreateInstance<TWindow>(_services);

        window.DataContext = viewModel;

        window.Closed += (_, _) => _openWindows.Remove(windowType);
        _openWindows[windowType] = window;

        window.Show();
    }
}