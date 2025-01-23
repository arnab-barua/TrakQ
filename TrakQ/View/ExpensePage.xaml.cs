namespace TrakQ.View;

public partial class ExpensePage : ContentPage
{
	public ExpensePage(ExpenseViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
        Appearing += OnAppearing;
    }

    private async void OnAppearing(object? sender, EventArgs e)
    {
        await (BindingContext as ExpenseViewModel)?.GetAllAsync();
    }
}