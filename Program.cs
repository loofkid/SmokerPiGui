using System;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.ReactiveUI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Projektanker.Icons.Avalonia;
using Projektanker.Icons.Avalonia.FontAwesome;
using SmokerPiGui.Web;
using SmokerPiGui.Web.Services;

namespace SmokerPiGui
{
    class Program
    {
        // Initialization code. Don't use any Avalonia, third-party APIs or any
        // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
        // yet and stuff might break.
        public static void Main(string[] args)
        {
            LoadServices();
            Task.Run(() => CreateHostBuilder(args).Build().RunAsync());
            BuildAvaloniaApp()
            .StartWithClassicDesktopLifetime(args);
        }

        // Avalonia configuration, don't remove; also used by visual designer.
        public static AppBuilder BuildAvaloniaApp()
            => AppBuilder.Configure<App>()
                .AfterSetup(AfterSetupCallback)
                .UsePlatformDetect()
                .LogToTrace()
                .UseReactiveUI()
                .With(new X11PlatformOptions() { EnableMultiTouch = true })
                .With(new MacOSPlatformOptions() { DisableDefaultApplicationMenuItems = true });

        
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });

        public static void AfterSetupCallback(AppBuilder appBuilder) =>
            IconProvider.Register<FontAwesomeIconProvider>();

        private static void LoadServices()
        {
            SmokerPiGuiServices.Services.AddSingleton<IProbeService, ProbeService>();
            SmokerPiGuiServices.Services.AddSingleton<IStatusService, StatusService>();

            SmokerPiGuiServices.ServiceProvider = SmokerPiGuiServices.Services.BuildServiceProvider();
        }
    }
}
