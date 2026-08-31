namespace TrakQ.View;
public static class InjectViews
{
    public static IServiceCollection AddViews(this IServiceCollection services)
    {
        services.AddSingleton<MainPage>();

        services.AddTransient<AccountSheetPage>();
        services.AddTransient<AccountSheetFormPage>();

        services.AddTransient<ExpenseHeadPage>();

        services.AddTransient<ExpensePage>();
        services.AddTransient<ExpenseFormPage>();


        services.AddTransient<IncomeHeadPage>();
        services.AddTransient<IncomeHeadFormPage>();

        services.AddTransient<IncomePage>();
        services.AddTransient<IncomeFormPage>();

        services.AddTransient<MonthSummeryPage>();

        return services;
    }
}
