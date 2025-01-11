namespace TrakQ.View;

public partial class IncomePage : ContentPage
{
	public IncomePage(IncomeViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
        viewModel.GetAllCommand.Execute(this);
    }
}