using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Windows.Forms;


namespace NTFSChecker
{
    internal static class Program
    {
        
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        private static void Main()
        {

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var serviceProvider = new ServiceCollection()
                .AddLogging(logging =>
                {
                    logging.AddConsole();
                    logging.AddDebug();
                })
                .BuildServiceProvider();

            var logger = serviceProvider.GetRequiredService<ILogger<MainForm>>();

            Application.Run(new MainForm(logger));
        }
    }
}