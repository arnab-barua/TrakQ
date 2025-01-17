namespace TrakQ.View;

public partial class IncomePage : ContentPage
{
	public IncomePage(IncomeViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
        Appearing += OnAppearing;
    }

    private void OnAppearing(object? sender, EventArgs e)
    {
        (BindingContext as IncomeViewModel)?.GetAllCommand.Execute(sender);
    }
}