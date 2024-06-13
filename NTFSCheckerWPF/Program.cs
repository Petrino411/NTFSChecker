using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NTFSChecker.Services;

namespace NTFSChecker
{
    public class Program
    {
        [STAThread]
        public static void Main()
        {
            // создаем хост приложения
            var host = Host.CreateDefaultBuilder()
                // внедряем сервисы
                .ConfigureServices(services =>
                {
                    services.AddSingleton<App>();
                    services.AddSingleton<MainWindow>();
                    services.AddSingleton<ExcelWriter>();
                    services.AddSingleton<UserGroupHelper>();
                    services.AddSingleton<DirectoryChecker>();

                })
                .Build();
            // получаем сервис - объект класса App
            var app = host.Services.GetService<App>();

            app?.Run();
        }
        
    }
}