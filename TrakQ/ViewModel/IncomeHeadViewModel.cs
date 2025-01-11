using TrakQ.Db.Data.Entities;
using TrakQ.Service;
using TrakQ.View;

namespace TrakQ.ViewModel;
public partial class IncomeHeadViewModel : BaseViewModel
{
    private readonly IncomeHeadService _incomeHeadService;
    public ObservableCollection<IncomeHead> IncomeHeads { get; set; } = [];



    public IncomeHeadViewModel(IncomeHeadService incomeHeadService)
    {
        Title = "Income Heads";
        _incomeHeadService = incomeHeadService;
    }


    [ObservableProperty]
    bool isRefreshing;


    [RelayCommand]
    async Task GetAllAsync()
    {
        if (IsBusy)
            return;

        try
        {
            IsBusy = true;
            var items = await _incomeHeadService.GetIncomeHeadsAsync();

            if (IncomeHeads.Count != 0)
                IncomeHeads.Clear();     

            foreach (var item in items)
            {
                IncomeHeads.Add(item);
            }

        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Unable to get monkeys: {ex.Message}");
            await Shell.Current.DisplayAlert("Error!", ex.Message, "OK");
        }
        finally
        {
            IsBusy = false;
            IsRefreshing = false;
        }

    }


    [RelayCommand]
    async Task CreateNew()
    {
        await GoToDetails(null);
    }

    [RelayCommand]
    async Task GoToDetails(IncomeHead? item)
    {
        if (item == null)
            item = new IncomeHead();

        await Shell.Current.GoToAsync(nameof(IncomeHeadFormPage), true, new Dictionary<string, object>
        {
            {"IncomeHead", item }
        });
    }
}
