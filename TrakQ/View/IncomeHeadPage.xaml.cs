using TrakQ.Db.Data.Entities;
using TrakQ.Service;

namespace TrakQ.View;

public partial class IncomeHeadPage : ContentPage
{
    private readonly IncomeHeadService _incomeHeadService;
    public ObservableCollection<IncomeHead> IncomeHeads { get; set; }

    public IncomeHeadPage(IncomeHeadService incomeHeadService)
    {
        InitializeComponent();

        _incomeHeadService = incomeHeadService;
        IncomeHeads = [];
        BindingContext = this;

        // Load the data when the page appears
        Loaded += OnPageLoaded;
    }

    private async void OnPageLoaded(object sender, EventArgs e)
    {
        await LoadIncomeHeadsAsync();
    }


    private async Task LoadIncomeHeadsAsync()
    {
        try
        {
            var items = await _incomeHeadService.GetIncomeHeadsAsync();
            IncomeHeads.Clear();

            foreach (var item in items)
            {
                IncomeHeads.Add(item);
            }
        }
        catch (Exception ex)
        {
            // Handle errors (e.g., show an alert)
            await DisplayAlert("Error", $"Failed to load income heads: {ex.Message}", "OK");
        }
    }


    private async void OnCreateNewClicked(object sender, EventArgs e)
    {
        var formPage = new IncomeHeadFormPage(_incomeHeadService);
        var viewModel = (IncomeHeadFormViewModel)formPage.BindingContext; // Explicit cast
        await viewModel.InitializeAsync(null); // Initialize with null for "Create" mode
        await Navigation.PushAsync(formPage);
    }

    private async void OnEditClicked(object sender, EventArgs e)
    {
        if (sender is Button button && button.CommandParameter is IncomeHead incomeHead)
        {
            var formPage = new IncomeHeadFormPage(_incomeHeadService);
            var viewModel = (IncomeHeadFormViewModel)formPage.BindingContext; // Explicit cast
            await viewModel.InitializeAsync(incomeHead); // Initialize with the selected expense head
            await Navigation.PushAsync(formPage);
        }
    }

}