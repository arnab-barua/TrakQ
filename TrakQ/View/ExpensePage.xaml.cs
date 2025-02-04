namespace TrakQ.View;

public partial class ExpensePage : ContentPage
{
    private readonly ExpenseViewModel _viewModel;
    public ExpensePage(ExpenseViewModel viewModel)
	{
		InitializeComponent();
        _viewModel = viewModel;
        BindingContext = viewModel;
        Appearing += OnAppearing;
    }

    private async void OnAppearing(object? sender, EventArgs e)
    {
        _viewModel.SetSelectedMonthAndYear();
        _viewModel.GetAllCommand.Execute(sender);
    }

    private void OnPickerValueChanged(object sender, EventArgs e)
    {
        _viewModel.OnMonthOrYearChanged();
        //_viewModel.GetAllCommand.Execute(sender);
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