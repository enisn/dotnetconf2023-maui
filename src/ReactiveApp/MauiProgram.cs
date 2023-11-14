using Microsoft.Extensions.Logging;
using ReactiveApp.Sources;
using ReactiveApp.ViewModels;
using ReactiveUI;
using System.Reactive;

namespace ReactiveApp;
public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        builder.Services
            .AddTransient<AppShell>()
            .AddTransient<AppShellViewModel>()
            .AddTransient<MainPage>()
            .AddTransient<MainViewModel>()
            .AddTransient<ProfilePage>()
            .AddTransient<ProfilePageViewModel>()
            .AddSingleton<ProfileDataSource>()
            ;

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}
