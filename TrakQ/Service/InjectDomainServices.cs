namespace TrakQ.Service;

public static class DependencyInjection
{
    public static IServiceCollection AddDomainServices(this IServiceCollection services)
    {
        services.AddSingleton<ExpenseHeadService>();
        services.AddSingleton<ExpenditureService>();
        
        services.AddSingleton<IncomeHeadService>();
        services.AddSingleton<IncomeService>();

        services.AddSingleton<ImportExportService>();

        return services;
    }
}
