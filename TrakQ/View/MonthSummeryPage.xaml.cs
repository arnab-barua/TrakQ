namespace TrakQ.View;

public partial class MonthSummeryPage : ContentPage
{
	public MonthSummeryPage(MonthSummeryPageViewModel viewModel)
	{
        InitializeComponent();
        BindingContext = viewModel;
        Appearing += OnAppearing;
    }

    private void OnAppearing(object? sender, EventArgs e)
    {
        (BindingContext as MonthSummeryPageViewModel)?.GetAllCommand.Execute(sender);
    }
}