using TrakQ.Dto;
using TrakQ.Service;

namespace TrakQ.ViewModel;
public partial class MonthSummeryPageViewModel : BaseViewModel
{
    private readonly ReportService _reportService;

    public MonthSummeryPageViewModel(ReportService reportService)
    {
        Title = "Month summery";
        _reportService = reportService;
        Month = Months.FirstOrDefault(a => a.Key == DateTime.Now.Month);
    }


    [ObservableProperty]
    bool isRefreshing;

    [ObservableProperty]
    MonthSummeryDto monthSummery = new();



    [ObservableProperty]
    public int year = DateTime.Now.Year;

    [ObservableProperty]
    public KeyValuePair<int, string> month = new(1, "January");

    public ObservableCollection<int> Years { get; set; } = [2022, 2023, 2024, 2025];
    public ObservableCollection<KeyValuePair<int, string>> Months { get; set; } = [
        new (1, "January"), 
        new (2, "February"),
        new (3, "March"),
        new (4, "April"),
        new (5, "May"),
        new (6, "June"),
        new (7, "July"),
        new (8, "August"),
        new (9, "September"),
        new (10, "October"),
        new (11, "November"),
        new (12, "December"),
    ];

    public async void MoveMonth(bool toLeft)
    {
        int currentMonth = Month.Key;
        if (toLeft && currentMonth == 1)
        {
            Month = Months.FirstOrDefault(a => a.Key == 12);
            Year = Year - 1;
        }
        else if (!toLeft && currentMonth == 12)
        {
            Month = Months.FirstOrDefault(a => a.Key == 1);
            Year = Year + 1;
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
    async Task GetAllAsync()
    {
        if (IsBusy)
            return;

        try
        {
            IsBusy = true;
            var item = await _reportService.GetMonthSummeryaAsync(Year, Month.Key);

            if (item is not null)
            {
                item.TotalSourceOfFund = item.TotalOpeningBalance + item.TotalIncome;
                item.RemainingBalance = item.TotalSourceOfFund - item.TotalExpense;
                item.Difference = item.TotalClosingBalance - item.RemainingBalance;
            }

            MonthSummery = item ?? new MonthSummeryDto();            
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

    
}
