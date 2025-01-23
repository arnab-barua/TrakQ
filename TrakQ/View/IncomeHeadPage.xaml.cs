namespace TrakQ.View;

public partial class IncomeHeadPage : ContentPage
{
	public IncomeHeadPage(IncomeHeadViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
        Appearing += OnAppearing;
    }

    private async void OnAppearing(object? sender, EventArgs e)
    {
        //(BindingContext as IncomeHeadViewModel)?.GetAllCommand.Execute(sender);
        await (BindingContext as IncomeHeadViewModel)?.GetAllAsync();
    }
}