namespace TrakQ.ViewModel;

public static class InjectViewModels
{
    public static IServiceCollection AddViewModels(this IServiceCollection services)
    {
        services.AddSingleton<ExpenseHeadViewModel>();
        services.AddSingleton<IncomeFormViewModel>();
        services.AddSingleton<IncomeHeadFormViewModel>();
        services.AddSingleton<IncomeHeadViewModel>();
        services.AddSingleton<IncomeViewModel>();

        return services;
    }
}
