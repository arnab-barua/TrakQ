namespace TrakQ.View;

public partial class AccountSheetPage : ContentPage
{
	public AccountSheetPage(AccountSheetPageViewModel viewModel)
	{
        InitializeComponent();
        BindingContext = viewModel;
        Appearing += OnAppearing;
    }

    private void OnAppearing(object? sender, EventArgs e)
    {
        (BindingContext as AccountSheetPageViewModel)?.GetAllCommand.Execute(sender);
    }
}