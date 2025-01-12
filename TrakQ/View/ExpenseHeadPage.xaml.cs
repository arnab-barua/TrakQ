namespace TrakQ.View;

public partial class ExpenseHeadPage : ContentPage
{
	public ExpenseHeadPage(ExpenseHeadViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
        viewModel.GetAllCommand.Execute(this);
    }
}