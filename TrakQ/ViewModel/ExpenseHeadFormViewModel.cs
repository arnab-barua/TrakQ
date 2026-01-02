using TrakQ.Dto;
using TrakQ.Service;

namespace TrakQ.ViewModel;

[QueryProperty(nameof(ExpenseHead), "ExpenseHead")]
public partial class ExpenseHeadFormViewModel : BaseViewModel
{
    private readonly ExpenseHeadService _expenseHeadService;

    public ExpenseHeadFormViewModel(ExpenseHeadService expenseHeadService)
    {
        Title = "Add/update expense head";
        _expenseHeadService = expenseHeadService;
    }

    [ObservableProperty]
    ExpenseHeadDto expenseHead = new();

    [RelayCommand]
    async Task SaveAsync()
    {
        if (IsBusy)
            return;

        try
        {
            IsBusy = true;

            bool isSuccess;
            if (ExpenseHead.Id == 0)
            {
                var id = await _expenseHeadService.AddExpenseHeadAsync(ExpenseHead);
                isSuccess = true;
            }
            else
            {
                await _expenseHeadService.UpdateExpenseHeadAsync(ExpenseHead);
                isSuccess = true;
            }

            if (isSuccess)
            {
                await Shell.Current.GoToAsync(".."); // Navigate back
            }

        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Unable to save expense head: {ex.Message}");
            await Shell.Current.DisplayAlert("Error!", ex.Message, "OK");
        }
        finally
        {
            IsBusy = false;
        }

    }

}
