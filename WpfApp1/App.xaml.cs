using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using WpfApp1.Services;

namespace WpfApp1
{
   
    public partial class App : Application
    {
        public static IHost? AppHost { get; private set; }
        

        public App()
        {
            AppHost = Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) =>
                {
                    services.AddSingleton<MainWindow>();
                    services.AddSingleton<IFileManager, FileManager>();
                    /* AddSingelton kör MainWindow och IFileManager en gång och inget mer. Detta eftersom programmet bara behöver startas en gång.*/
                }).Build();
        }


        protected override async void OnStartup(StartupEventArgs e)
        {
            await AppHost!.StartAsync();
            var startupWindow = AppHost.Services.GetRequiredService<MainWindow>();
            startupWindow.Show();

            base.OnStartup(e);
            /* Eftersom jag har tagit bort programmets vanliga väg att startas så måste jag hör override det som körs när programmet startar.*/
        }

        protected override async void OnExit(ExitEventArgs e)
        {
            await AppHost!.StopAsync();
            base.OnExit(e);
            /* Samma som vid start fast vid avslut.*/
        }

    }
}
