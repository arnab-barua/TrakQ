namespace TrakQ.View;
public static class InjectViews
{
    public static IServiceCollection AddViews(this IServiceCollection services)
    {
        services.AddSingleton<MainPage>();

        services.AddSingleton<AccountSheetPage>();
        services.AddSingleton<AccountSheetFormPage>();

        services.AddSingleton<ExpenseHeadPage>();

        services.AddSingleton<ExpensePage>();
        services.AddSingleton<ExpenseFormPage>();


        services.AddSingleton<IncomeHeadPage>();
        services.AddSingleton<IncomeHeadFormPage>();

        services.AddSingleton<IncomePage>();
        services.AddSingleton<IncomeFormPage>();

        services.AddSingleton<MonthSummeryPage>();

        return services;
    }
}
