namespace TrakQ.View;

public partial class ExpenseHeadFormPage : ContentPage
{
	public ExpenseHeadFormPage(ExpenseHeadFormViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}