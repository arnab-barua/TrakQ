﻿using Fonts;
using Microsoft.Extensions.Logging;
using TrakQ.Db;
using TrakQ.Service;
using TrakQ.View;

namespace TrakQ;

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

#if DEBUG
		builder.Logging.AddDebug();
#endif

        builder.Services.AddDbContext<AppDbContext>();

        builder.Services.AddDomainServices();
        builder.Services.AddViewModels();
        builder.Services.AddViews();

        return builder.Build();
    }
}
