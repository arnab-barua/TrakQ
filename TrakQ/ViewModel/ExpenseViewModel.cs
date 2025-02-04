using TrakQ.Dto;
using TrakQ.Service;
using TrakQ.View;

namespace TrakQ.ViewModel;
public partial class ExpenseViewModel : BaseViewModel
{
    private readonly ExpenditureService _expenditureService;
    private readonly MonthBoardService _monthBoardService;
    public ObservableCollection<ExpensesByDay> Expenses { get; set; } = [];



    public ExpenseViewModel(ExpenditureService expenditureService, MonthBoardService monthBoardService)
    {
        Title = "Expenses";
        _expenditureService = expenditureService;
        _monthBoardService = monthBoardService;
        SetMonthsAndYearsFromService();
    }


    [ObservableProperty]
    bool isRefreshing;

    [ObservableProperty]
    decimal total;


    [ObservableProperty]
    KeyValuePair<int, string> year;

    [ObservableProperty]
    KeyValuePair<int, string> month;

    public ObservableCollection<KeyValuePair<int, string>> Months { get; set; }
    public ObservableCollection<KeyValuePair<int, string>> Years { get; set; }

    private void SetMonthsAndYearsFromService()
    {
        Months = _monthBoardService.Months;
        Years = _monthBoardService.Years;
    }

    /// <summary>
    /// Service => ViewModel.
    /// </summary>
    public void SetSelectedMonthAndYear()
    {
        Month = _monthBoardService.SelectedMonth;
        Year = _monthBoardService.SelectedYear;
    }

    /// <summary>
    /// ViewModel => Service.
    /// </summary>
    public async void OnMonthOrYearChanged()
    {
        if(Month.Key > 0 && Year.Key > 0)
        {
            _monthBoardService.SetMonthAndYear(Month, Year);
            await GetAllAsync();
        }        
    }

    public async void MoveMonth(bool toLeft)
    {
        int currentMonth = Month.Key;
        int currentYear = Year.Key;
        if (toLeft && currentMonth == 1)
        {
            Month = Months.FirstOrDefault(a => a.Key == 12);
            Year = Years.FirstOrDefault(a => a.Key == currentYear - 1);
        }
        else if (!toLeft && currentMonth == 12)
        {
            Month = Months.FirstOrDefault(a => a.Key == 1);
            Year = Years.FirstOrDefault(a => a.Key == currentYear + 1);
        }
        else if (toLeft)
        {
            Month = Months.FirstOrDefault(a => a.Key == currentMonth - 1);
        }
        else
        {
            Month = Months.FirstOrDefault(a => a.Key == currentMonth + 1);
        }
    }


    [RelayCommand]
    public async Task GetAllAsync()
    {
        if (IsBusy)
            return;

        try
        {
            IsBusy = true;
            var items = await _expenditureService.GetMonthDataAsync(Year.Key, Month.Key);

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
