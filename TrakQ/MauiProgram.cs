using Fonts;
using Microsoft.Extensions.Logging;
using TrakQ.Db;
using TrakQ.Service;
using TrakQ.View;
using UraniumUI;
using Microsoft.Maui.Controls.Compatibility.Hosting;

namespace TrakQ;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        // Wire up early global exception handlers to capture any startup crash
        AppDomain.CurrentDomain.UnhandledException += (s, e) =>
        {
            if (e.ExceptionObject is Exception ex)
            {
                ExceptionLoggerService.Log(ex);
            }
        };

        TaskScheduler.UnobservedTaskException += (s, e) =>
        {
            if (e.Exception is Exception ex)
            {
                ExceptionLoggerService.Log(ex);
            }
            e.SetObserved();
        };

        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseUraniumUI()
            .UseUraniumUIMaterial()
            .UseMauiCompatibility()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                fonts.AddFontAwesomeIconFonts();
            });

#if DEBUG
		builder.Logging.AddDebug();
#endif

        builder.Services.AddDbContextFactory<AppDbContext>();
        builder.Services.AddDomainServices();
        builder.Services.AddViewModels();
        builder.Services.AddViews();

        return builder.Build();
    }
}

