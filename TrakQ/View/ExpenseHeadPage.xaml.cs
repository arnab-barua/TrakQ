using TrakQ.Dto;
using TrakQ.Service;

namespace TrakQ.View;

public partial class ExpenseHeadPage : ContentPage
{
    private readonly ExpenseHeadService _expenseHeadService;
    public ObservableCollection<ExpenseHeadDto> ExpenseHeads { get; set; }

    public ExpenseHeadPage(ExpenseHeadService expenseHeadService)
    {
        InitializeComponent();

        _expenseHeadService = expenseHeadService;
        ExpenseHeads = [];
        BindingContext = this;

        // Load the data when the page appears
        Loaded += OnPageLoaded;
    }



    private async void OnPageLoaded(object sender, EventArgs e)
    {
        await LoadExpenseHeadsAsync();
    }

    private async Task LoadExpenseHeadsAsync()
    {
        try
        {
            var items = await _expenseHeadService.GetExpenseHeadsAsync();
            ExpenseHeads.Clear();

            foreach (var item in items)
            {
                ExpenseHeads.Add(item);
            }
        }
        catch (Exception ex)
        {
            // Handle errors (e.g., show an alert)
            await DisplayAlert("Error", $"Failed to load expense heads: {ex.Message}", "OK");
        }
    }


    private async void OnCreateNewClicked(object sender, EventArgs e)
    {
        var formPage = new ExpenseHeadFormPage(_expenseHeadService);
        var viewModel = (ExpenseHeadFormViewModel)formPage.BindingContext; // Explicit cast
        await viewModel.InitializeAsync(null); // Initialize with null for "Create" mode
        await Navigation.PushAsync(formPage);
    }

    private async void OnEditClicked(object sender, EventArgs e)
    {
        if (sender is Button button && button.CommandParameter is ExpenseHeadDto expenseHead)
        {
            var formPage = new ExpenseHeadFormPage(_expenseHeadService);
            var viewModel = (ExpenseHeadFormViewModel)formPage.BindingContext; // Explicit cast
            await viewModel.InitializeAsync(expenseHead); // Initialize with the selected expense head
            await Navigation.PushAsync(formPage);
        }
    }
}