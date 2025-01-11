using TrakQ.Service;

namespace TrakQ.View;

public partial class ExpenseHeadFormPage : ContentPage
{
    private readonly ExpenseHeadService _expenseHeadService;

    public ExpenseHeadFormPage(ExpenseHeadService expenseHeadService)
    {
        InitializeComponent();
        _expenseHeadService = expenseHeadService;
        BindingContext = new ExpenseHeadFormViewModel(_expenseHeadService);
    }
}