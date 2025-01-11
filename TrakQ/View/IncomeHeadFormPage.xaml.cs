using TrakQ.Service;

namespace TrakQ.View;

public partial class IncomeHeadFormPage : ContentPage
{
    private readonly IncomeHeadService _incomeHeadService;

    public IncomeHeadFormPage(IncomeHeadService incomeHeadService)
    {
        InitializeComponent();
        _incomeHeadService = incomeHeadService;
        BindingContext = new IncomeHeadFormViewModel(_incomeHeadService);
    }
}