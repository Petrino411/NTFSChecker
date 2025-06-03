using System;
using System.IO;
using System.Runtime.InteropServices;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NTFSChecker.AvaloniaUI.Interfaces;
using NTFSChecker.AvaloniaUI.Services;
using NTFSChecker.AvaloniaUI.ViewModels;
using NTFSChecker.AvaloniaUI.Views;
using NTFSChecker.Core.Interfaces;
using NTFSChecker.Core.Services;
using NTFSChecker.Linux.Services;
using NTFSChecker.Windows.Services;

namespace NTFSChecker.AvaloniaUI;

public static class Bootstrapper
{
    public static IServiceProvider Configure()
    {
        var services = new ServiceCollection();

        services.AddSingleton<MainWindowViewModel>();
        services.AddSingleton<MainPageViewModel>();
        services.AddSingleton<StatisticsPageViewModel>();
        services.AddSingleton<SettingsWinViewModel>();

        services.AddSingleton<SettingsForm>();
        services.AddSingleton<MainWindow>();
        
        services.AddScoped<IExcelWriter, ExcelWriter>();
        services.AddSingleton<IWindowService, WindowService>();
        services.AddSingleton<ISettingsService, SettingsService>();

        #region Windows
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            services.AddScoped<IDirectoryChecker, WindowsDirectoryChecker>();
            services.AddSingleton<IUserGroupHelper, WindowsUserGroupHelper>();
            services.AddSingleton<INetworkPathResolver, WindowsNetworkPathResolver>();
        }
        #endregion

        #region Linux
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        {
            services.AddScoped<IDirectoryChecker, LinuxDirectoryChecker>();
            services.AddSingleton<INetworkPathResolver, LinuxNetworkPathResolver>();
            services.AddSingleton<IUserGroupHelper, LinuxUserGroupHelper>();
        }
        #endregion

        services.AddLogging(configure => configure.AddConsole());
        
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

        services.AddSingleton(builder.Build());
        
        return services.BuildServiceProvider();
    }
}