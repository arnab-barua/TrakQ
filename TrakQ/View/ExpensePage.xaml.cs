namespace TrakQ.View;

public partial class ExpensePage : ContentPage
{
	public ExpensePage(ExpenseViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
        viewModel.GetAllCommand.Execute(this);
	}
}