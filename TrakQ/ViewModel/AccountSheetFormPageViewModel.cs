using TrakQ.Dto;
using TrakQ.Service;

namespace TrakQ.ViewModel;

[QueryProperty("AccountSheet", "AccountSheet")]
public partial class AccountSheetFormPageViewModel : BaseViewModel
{
    private readonly AccountSheetService _accountSheetService;
    private readonly AccountService _accountService;
    private readonly MonthBoardService _monthBoardService;


    public AccountSheetFormPageViewModel(AccountSheetService accountSheetService, AccountService accountService, MonthBoardService monthBoardService)
    {
        Title = "Add/update account";
        _accountSheetService = accountSheetService;
        _accountService = accountService;
        _monthBoardService = monthBoardService;
        SetMonthsAndYearsFromService();
    }


    public ObservableCollection<KeyValuePair<int, string>> Accounts { get; set; } = [];



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



    [ObservableProperty]
    AccountSheetDto accountSheet = new();

    [ObservableProperty]
    KeyValuePair<int, string> selectedAccount = new();

    [RelayCommand]
    async Task SaveAsync()
    {
        if (IsBusy)
            return;

        try
        {
            IsBusy = true;

            if (SelectedAccount.Key == null)
            {
                await Shell.Current.DisplayAlert("Error!", "Select expense head", "OK");
                return;
            }

            AccountSheet.AccountId = SelectedAccount.Key;
            AccountSheet.Month = (byte)Month.Key;
            AccountSheet.Year = (short)Year.Key;

            int changedId = 0;
            if (AccountSheet.Id == 0)
            {
                changedId = await _accountSheetService.AddAsync(AccountSheet);
            }
            else
            {
                changedId = await _accountSheetService.UpdateAsync(AccountSheet);
            }

            if (changedId > 0)
            {
                await Shell.Current.GoToAsync(".."); // Navigate back
            }

        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Unable to save income: {ex.Message}");
            await Shell.Current.DisplayAlert("Error!", ex.Message, "OK");
        }
        finally
        {
            IsBusy = false;
        }

    }

    [RelayCommand]
    async Task GetAllAccountsAsync()
    {
        try
        {
            IsBusy = true;
            var data = AccountSheet;

            var items = await _accountService.GetAllAccountsAsync();

            if (Accounts.Count != 0)
                Accounts.Clear();

            foreach (var item in items)
            {
                Accounts.Add(item);
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Unable to get income heads: {ex.Message}");
            await Shell.Current.DisplayAlert("Error!", ex.Message, "OK");
        }
        finally
        {
            IsBusy = false;
        }
    }

    public void SetPickerValues()
    {
        if (Accounts.Count != 0 && AccountSheet.AccountId > 0)
        {
            SelectedAccount = Accounts.FirstOrDefault(h => h.Key == AccountSheet.AccountId);
        }
        else
        {
            SelectedAccount = new();
        }

        if (Months.Count != 0 && AccountSheet.Month > 0)
        {
            Month = Months.FirstOrDefault(h => h.Key == AccountSheet.Month);
        }

        if (Years.Count != 0 && AccountSheet.Year > 0)
        {
            Year = Years.FirstOrDefault(h => h.Key == AccountSheet.Year);
        }

        
    }
}
