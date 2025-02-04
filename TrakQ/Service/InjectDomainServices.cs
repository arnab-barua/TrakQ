namespace TrakQ.Service;

public static class DependencyInjection
{
    public static IServiceCollection AddDomainServices(this IServiceCollection services)
    {
        services.AddSingleton<AccountService>();
        services.AddSingleton<AccountSheetService>();

        services.AddSingleton<ExpenseHeadService>();
        services.AddSingleton<ExpenditureService>();

        services.AddSingleton<FiscalMonthService>();

        services.AddSingleton<IncomeHeadService>();
        services.AddSingleton<IncomeService>();

        services.AddSingleton<ImportExportService>();

        services.AddSingleton<ReportService>();
        
        services.AddSingleton<MonthBoardService>();

        return services;
    }
}
