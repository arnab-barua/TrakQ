namespace TrakQ.View;

public partial class AccountSheetFormPage : ContentPage
{
    private readonly AccountSheetFormPageViewModel _viewModel;
    public AccountSheetFormPage(AccountSheetFormPageViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
        _viewModel = viewModel;
        _viewModel.GetAllAccountsCommand.Execute(this);

        Appearing += OnAppearing;
    }

    private void OnAppearing(object? sender, EventArgs e)
    {
        _viewModel.SetPickerValues();
    }
}