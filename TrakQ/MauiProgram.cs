﻿using Microsoft.Extensions.Logging;
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

        builder.Services.AddSingleton<ExpenseHeadService>();

        builder.Services.AddSingleton<IncomeHeadService>();
        builder.Services.AddSingleton<IncomeHeadViewModel>();
        builder.Services.AddTransient<IncomeHeadPage>();

        builder.Services.AddSingleton<IncomeHeadFormViewModel>();
        builder.Services.AddSingleton<IncomeHeadFormPage>();

        return builder.Build();
    }
}
