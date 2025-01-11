namespace TrakQ.View;

public partial class IncomeHeadPage : ContentPage
{
	public IncomeHeadPage(IncomeHeadViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
        viewModel.GetAllCommand.Execute(this);
	}
}