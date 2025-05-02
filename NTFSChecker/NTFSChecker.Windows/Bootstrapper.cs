using System;
using System.Configuration;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NTFSChecker.Windows.Models;
using NTFSChecker.Windows.Services;
using NTFSChecker.Windows.ViewModels;
using NTFSChecker.Windows.Views;

public static class Bootstrapper
{
    public static IServiceProvider Configure()
    {
        var services = new ServiceCollection();

        // ViewModels
        services.AddSingleton<MainWindowViewModel>();
        services.AddSingleton<SettingsWinViewModel>();

        // Views
        services.AddSingleton<SettingsForm>();
        services.AddSingleton<MainWindow>();

        // Services
        services.AddSingleton<ExcelWriter>();
        services.AddSingleton<DirectoryChecker>();
        services.AddSingleton<UserGroupHelper>();
        services.AddSingleton<IWindowService, WindowService>();

        // Logging
        services.AddLogging(configure => configure.AddConsole());
        
        services.AddSingleton<SettingsService>();
        
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
        
        services.AddSingleton(builder.Build());
        

        return services.BuildServiceProvider();
    }
}