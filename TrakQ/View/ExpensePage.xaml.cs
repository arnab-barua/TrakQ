namespace TrakQ.View;

public partial class ExpensePage : ContentPage
{
	public ExpensePage(ExpenseViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
        Appearing += OnAppearing;
    }

    private void OnAppearing(object? sender, EventArgs e)
    {
        (BindingContext as ExpenseViewModel)?.GetAllCommand.Execute(sender);
    }
}