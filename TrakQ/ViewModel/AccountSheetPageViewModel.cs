using TrakQ.Dto;
using TrakQ.Service;
using TrakQ.View;

namespace TrakQ.ViewModel;
public partial class AccountSheetPageViewModel : BaseViewModel
{
    private readonly AccountSheetService _accountSheetService;
    private readonly MonthBoardService _monthBoardService;
    [ObservableProperty] ObservableCollection<AccountSheetDto> accountSheets = [];



    public AccountSheetPageViewModel(AccountSheetService expenditureService, MonthBoardService monthBoardService)
    {
        Title = "Account sheet";
        _accountSheetService = expenditureService;
        _monthBoardService = monthBoardService;
        SetMonthsAndYearsFromService();
    }


    [ObservableProperty]
    bool isRefreshing;

    [ObservableProperty]
    decimal totalOpening;

    [ObservableProperty]
    decimal totalClosing;


    [ObservableProperty]
    KeyValuePair<int, string> year;
    partial void OnYearChanged(KeyValuePair<int, string> value) { _ = OnMonthOrYearChanged(); }

    [ObservableProperty]
    KeyValuePair<int, string> month;
    partial void OnMonthChanged(KeyValuePair<int, string> value) { _ = OnMonthOrYearChanged(); }

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
    public async Task OnMonthOrYearChanged()
    {
        if (Month.Key > 0 && Year.Key > 0)
        {
            _monthBoardService.SetMonthAndYear(Month, Year);
            await GetAllAsync();
        }
    }

    public void MoveMonth(bool toLeft)
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
            var items = await _accountSheetService.GetAccountBalancesAsync(Year.Key, Month.Key);

            

            AccountSheets = new ObservableCollection<AccountSheetDto>(items);
            TotalOpening = items.Sum(a => a.OpeningBalance);
            TotalClosing = items.Sum(a => a.ClosingBalance) ?? 0;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Unable to get account sheets: {ex.Message}");
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
    async Task GoToDetails(AccountSheetDto? item)
    {
        item ??= new AccountSheetDto();

        await Shell.Current.GoToAsync(nameof(AccountSheetFormPage), true, new Dictionary<string, object>
        {
            {"AccountSheet", item }
        });
    }
}



