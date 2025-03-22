using System;
using System.IO;
using System.Text;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.Configuration;
using NTFSChecker.Avalonia.Services;

namespace NTFSChecker.Avalonia;

public partial class App : Application
{
    public static IServiceProvider ServiceProvider { get; set; }
    public static IConfiguration Configuration { get; private set; }

    public override void Initialize()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

        Configuration = builder.Build();
        
        var services = new ServiceCollection();
        ConfigureServices(services);
        ServiceProvider = services.BuildServiceProvider();

        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = ServiceProvider.GetRequiredService<MainWindow>();
        }

        base.OnFrameworkInitializationCompleted();
    }

    private static void ConfigureServices(IServiceCollection services)
    {
        services.AddSingleton(Configuration);
        
        services.AddLogging(builder =>
        {
            builder.AddConsole();
            builder.AddDebug();
        });
        
        services.AddTransient<MainWindow>();
        services.AddTransient<SettingsForm>();
        services.AddSingleton<ExcelWriter>();
        services.AddSingleton<UserGroupHelper>();
        services.AddSingleton<DirectoryChecker>();
    }
}