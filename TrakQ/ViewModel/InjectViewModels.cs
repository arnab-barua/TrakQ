namespace TrakQ.ViewModel;

public static class InjectViewModels
{
    public static IServiceCollection AddViewModels(this IServiceCollection services)
    {
        services.AddSingleton<MainPageViewModel>();

        services.AddTransient<AccountSheetFormPageViewModel>();
        services.AddTransient<AccountSheetPageViewModel>();

        services.AddTransient<ExpenseHeadViewModel>();
        services.AddTransient<ExpenseHeadFormViewModel>();
        
        services.AddTransient<ExpenseViewModel>();
        services.AddTransient<ExpenseFormViewModel>();
        
        services.AddTransient<IncomeHeadViewModel>();
        services.AddTransient<IncomeHeadFormViewModel>();
        
        services.AddTransient<IncomeViewModel>();
        services.AddTransient<IncomeFormViewModel>();

        services.AddTransient<MonthSummeryPageViewModel>();

        services.AddTransient<LogsPageViewModel>();
        return services;
    }
}

