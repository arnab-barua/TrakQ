using TrakQ.Db.Data.Entities;
using TrakQ.Dto;
using TrakQ.Service;
using TrakQ.View;

namespace TrakQ.ViewModel;
public partial class ExpenseHeadViewModel : BaseViewModel
{
    private readonly ExpenseHeadService _expenseHeadService;

    public ExpenseHeadViewModel(ExpenseHeadService expenseHeadService)
    {
        Title = "Expense heads";
        _expenseHeadService = expenseHeadService;
    }

    public ObservableCollection<ExpenseHeadDto> ExpenseHeads { get; set; } = [];    


    [ObservableProperty]
    bool isRefreshing;


    [RelayCommand]
    async Task GetAllAsync()
    {
        if (IsBusy)
            return;

        try
        {
            IsBusy = true;
            var items = await _expenseHeadService.GetAllAsync();

            if (ExpenseHeads.Count != 0)
                ExpenseHeads.Clear();     

            foreach (var item in items)
            {
                ExpenseHeads.Add(item);
            }

        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Unable to get expense heads: {ex.Message}");
            await Shell.Current.DisplayAlert("Error!", ex.Message, "OK");
        }
        finally
        {
            IsBusy = false;
            IsRefreshing = false;
        }
    }
}
