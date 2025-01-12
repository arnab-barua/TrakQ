namespace TrakQ.View;

public partial class ExpenseHeadPage : ContentPage
{
	public ExpenseHeadPage(ExpenseHeadViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
        Appearing += OnAppearing;
    }

    private void OnAppearing(object? sender, EventArgs e)
    {
        (BindingContext as ExpenseHeadViewModel)?.GetAllCommand.Execute(sender);
    }
}