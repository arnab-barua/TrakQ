namespace TrakQ.Service;

public static class DependencyInjection
{
    public static IServiceCollection AddDomainServices(this IServiceCollection services)
    {
        services.AddSingleton<ExpenditureService>();
        services.AddSingleton<ExpenseHeadService>();
        services.AddSingleton<IncomeHeadService>();
        services.AddSingleton<IncomeService>();

        return services;
    }
}
