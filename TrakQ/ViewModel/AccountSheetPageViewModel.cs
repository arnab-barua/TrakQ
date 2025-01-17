using TrakQ.Dto;
using TrakQ.Service;

namespace TrakQ.ViewModel;
public partial class AccountSheetPageViewModel : BaseViewModel
{
    private readonly AccountSheetService _accountSheetService;
    public ObservableCollection<AccountSheetDto> AccountSheets { get; set; } = [];



    public AccountSheetPageViewModel(AccountSheetService expenditureService)
    {
        Title = "Account sheet";
        _accountSheetService = expenditureService;
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
            var items = await _accountSheetService.GetAccountBalancesAsync(Year, Month);

            

            if (AccountSheets.Count != 0)
                AccountSheets.Clear();

            foreach (var item in items)
            {
                AccountSheets.Add(item);
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
    async Task GoToDetails(AccountSheetDto? item)
    {
        //item ??= new AccountSheetDto();

        //await Shell.Current.GoToAsync(nameof(ExpenseFormPage), true, new Dictionary<string, object>
        //{
        //    {"Expenditure", item }
        //});
    }
}
