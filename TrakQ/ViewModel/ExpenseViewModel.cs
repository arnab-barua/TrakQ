using TrakQ.Dto;
using TrakQ.Service;
using TrakQ.View;

namespace TrakQ.ViewModel;
public partial class ExpenseViewModel : BaseViewModel
{
    private readonly ExpenditureService _expenditureService;
    public ObservableCollection<ExpensesByDay> Expenses { get; set; } = [];



    public ExpenseViewModel(ExpenditureService expenditureService)
    {
        Title = "Expenses";
        _expenditureService = expenditureService;
    }


    [ObservableProperty]
    bool isRefreshing;

    [ObservableProperty]
    decimal total;


    int Year = DateTime.Now.Year;

    int Month = DateTime.Now.Month;


    [RelayCommand]
    public async Task GetAllAsync()
    {
        if (IsBusy)
            return;

        try
        {
            IsBusy = true;
            var items = await _expenditureService.GetMonthDataAsync(Year, Month);

            List<ExpensesByDay> newItems = items.GroupBy(a => a.ExpenditureDate)
                .Select(gr => new ExpensesByDay
                {
                    ExpenditureDate = gr.Key,
                    Expenses = [.. gr],
                    TotalAmount = gr.Sum(a => a.Amount)
                })
                .OrderBy(a => a.ExpenditureDate)
                .ToList();

            if (Expenses.Count != 0)
                Expenses.Clear();

            foreach (var item in newItems)
            {
                Expenses.Add(item);
            }
            Total = newItems.Sum(a => a.TotalAmount); 
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
    async Task GoToDetails(ExpenditureDto? item)
    {
        item ??= new ExpenditureDto();

        await Shell.Current.GoToAsync(nameof(ExpenseFormPage), true, new Dictionary<string, object>
        {
            {"Expenditure", item }
        });
    }
}
