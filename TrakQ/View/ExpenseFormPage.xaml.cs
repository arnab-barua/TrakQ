namespace TrakQ.View;

public partial class ExpenseFormPage : ContentPage
{
	public ExpenseFormPage(ExpenseFormViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
        viewModel.GetAllExpenseHeadsCommand.Execute(this);

        Appearing += OnAppearing;
    }

    private void OnAppearing(object? sender, EventArgs e)
    {
        (BindingContext as ExpenseFormViewModel)?.SetExpenseHead();
    }
}