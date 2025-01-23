namespace TrakQ.View;

public partial class ExpenseHeadPage : ContentPage
{
	public ExpenseHeadPage(ExpenseHeadViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
        Appearing += OnAppearing;
    }

    private async void OnAppearing(object? sender, EventArgs e)
    {
        await (BindingContext as ExpenseHeadViewModel)?.GetAllAsync();
    }
}