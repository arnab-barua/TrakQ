using TrakQ.Dto;
using TrakQ.Service;

namespace TrakQ.ViewModel;
public partial class ExpenseViewModel : BaseViewModel
{
    private readonly ExpenditureService _expenditureService;
    public ObservableCollection<ExpenditureDto> Expenses { get; set; } = [];



    public ExpenseViewModel(ExpenditureService expenditureService)
    {
        Title = "Expenses";
        _expenditureService = expenditureService;
    }


    [ObservableProperty]
    bool isRefreshing;


    int Year = DateTime.Now.Year;

    int Month = DateTime.Now.Month;


    [RelayCommand]
    async Task GetAllAsync()
    {
        if (IsBusy)
            return;

        try
        {
            IsBusy = true;
            var items = await _expenditureService.GetMonthDataAsync(Year, Month);

            if (Expenses.Count != 0)
                Expenses.Clear();

            foreach (var item in items)
            {
                Expenses.Add(item);
            }

        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Unable to get incomes: {ex.Message}");
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
    async Task GoToDetails(IncomeViewDto? item)
    {
        if (item == null)
            item = new IncomeViewDto();

        //await Shell.Current.GoToAsync(nameof(IncomeFormPage), true, new Dictionary<string, object>
        //{
        //    {"Income", item }
        //});
    }
}
