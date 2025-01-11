
namespace TrakQ.View;

public partial class IncomeFormPage : ContentPage
{
    public IncomeFormPage(IncomeFormViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
        viewModel.GetAllIncomeHeadsCommand.Execute(this);

        //Loaded += OnPageLoaded;
        Appearing += OnAppearing;
    }

    private void OnAppearing(object? sender, EventArgs e)
    {
        (BindingContext as IncomeFormViewModel)?.SetIncomeHead();
    }

    private async void OnPageLoaded(object sender, EventArgs e)
    {
        (BindingContext as IncomeFormViewModel)?.SetIncomeHead();
    }
}