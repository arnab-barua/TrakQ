using System.Collections.Generic;
using TrakQ.Dto;
using TrakQ.Service;

namespace TrakQ.ViewModel;
public partial class MonthSummeryPageViewModel : BaseViewModel
{
    private readonly ReportService _reportService;
    private readonly MonthBoardService _monthBoardService;

    public MonthSummeryPageViewModel(ReportService reportService, MonthBoardService monthBoardService)
    {
        Title = "Month summery";
        _reportService = reportService;
        _monthBoardService = monthBoardService;
        SetMonthsAndYearsFromService();
    }


    [ObservableProperty]
    bool isRefreshing;

    [ObservableProperty]
    MonthSummeryDto monthSummery = new();



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
        if (Month.Key > 0 && Year.Key > 0)
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
    async Task GetAllAsync()
    {
        if (IsBusy)
            return;

        try
        {
            IsBusy = true;
            var item = await _reportService.GetMonthSummeryaAsync(Year.Key, Month.Key);

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
