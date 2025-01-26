namespace TrakQ.View;

public partial class MonthSummeryPage : ContentPage
{
    private readonly MonthSummeryPageViewModel _viewModel;
    public MonthSummeryPage(MonthSummeryPageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
        _viewModel = viewModel;
        Appearing += OnAppearing;
    }

    private void OnAppearing(object? sender, EventArgs e)
    {
        _viewModel.GetAllCommand.Execute(sender);
    }

    private void OnPickerValueChanged(object sender, EventArgs e)
    {
        _viewModel.GetAllCommand.Execute(sender);
    }

    private void OnPreviousMonthMOveClicked(object sender, EventArgs e)
    {
        _viewModel?.MoveMonth(true);
    }

    private void OnNextMonthMOveClicked(object sender, EventArgs e)
    {
        _viewModel?.MoveMonth(false);
    }
}