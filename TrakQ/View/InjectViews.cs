namespace TrakQ.View;
public static class InjectViews
{
    public static IServiceCollection AddViews(this IServiceCollection services)
    {
        services.AddSingleton<ExpenseHeadPage>();

        services.AddSingleton<ExpensePage>();


        services.AddSingleton<IncomeHeadPage>();
        services.AddSingleton<IncomeHeadFormPage>();

        services.AddSingleton<IncomePage>();
        services.AddSingleton<IncomeFormPage>();

        return services;
    }
}
