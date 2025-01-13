namespace TrakQ.ViewModel;

public static class InjectViewModels
{
    public static IServiceCollection AddViewModels(this IServiceCollection services)
    {
        services.AddSingleton<MainPageViewModel>();
        
        services.AddSingleton<ExpenseHeadViewModel>();
        
        services.AddSingleton<ExpenseViewModel>();
        services.AddSingleton<ExpenseFormViewModel>();
        
        services.AddSingleton<IncomeHeadViewModel>();
        services.AddSingleton<IncomeHeadFormViewModel>();
        
        services.AddSingleton<IncomeViewModel>();
        services.AddSingleton<IncomeFormViewModel>();

        return services;
    }
}
