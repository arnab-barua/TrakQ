namespace TrakQ.View;

public partial class AccountSheetPage : ContentPage
{
    private readonly AccountSheetPageViewModel _viewModel;
    public AccountSheetPage(AccountSheetPageViewModel viewModel)
	{
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = viewModel;
        Appearing += OnAppearing;
    }

    private void OnAppearing(object? sender, EventArgs e)
    {
        _viewModel.SetSelectedMonthAndYear();
        _viewModel.GetAllCommand.Execute(sender);
    }

    private async void OnPickerValueChanged(object sender, EventArgs e)
    {
        await _viewModel.OnMonthOrYearChanged();
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
