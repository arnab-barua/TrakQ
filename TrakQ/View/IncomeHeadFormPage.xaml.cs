namespace TrakQ.View;

public partial class IncomeHeadFormPage : ContentPage
{
	public IncomeHeadFormPage(IncomeHeadFormViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
	}
}