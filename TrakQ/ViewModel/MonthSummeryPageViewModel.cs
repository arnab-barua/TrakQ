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
    }


    [ObservableProperty]
    bool isRefreshing;

    [ObservableProperty]
    MonthSummeryDto monthSummery = new();

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
            var item = await _reportService.GetMonthSummeryaAsync(Year, Month);

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
