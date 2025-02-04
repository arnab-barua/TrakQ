using TrakQ.Dto;
using TrakQ.Service;

namespace TrakQ.ViewModel;

[QueryProperty("Expenditure", "Expenditure")]
public partial class ExpenseFormViewModel : BaseViewModel
{
    private readonly ExpenditureService _expenditureService;
    private readonly ExpenseHeadService _expenseHeadService;

    public ExpenseFormViewModel(ExpenditureService expenditureService, ExpenseHeadService expenseHeadService)
    {
        Title = "Add/update expenditure";
        _expenditureService = expenditureService;
        _expenseHeadService = expenseHeadService;
    }


    public ObservableCollection<ExpenseHeadShortDto> ExpenseHeads { get; set; } = [];


    [ObservableProperty]
    ExpenditureDto expenditure = new();

    [ObservableProperty]
    ExpenseHeadShortDto selectedExpenseHead = new();

    [RelayCommand]
    async Task SaveAsync()
    {
        if (IsBusy)
            return;

        try
        {
            IsBusy = true;

            if (SelectedExpenseHead?.Id == null)
            {
                await Shell.Current.DisplayAlert("Error!", "Select expense head", "OK");
                return;
            }

            Expenditure.ExpenditureHeadId = SelectedExpenseHead.Id;
            Expenditure.ExpenditureDate = Expenditure.ExpenditureDate.Date;

            int changedId = 0;
            if (Expenditure.ExpenditureId == 0)
            {
                changedId = await _expenditureService.AddAsync(Expenditure);
            }
            else
            {
                changedId = await _expenditureService.UpdateAsync(Expenditure);
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
    async Task GetAllExpenseHeadsAsync()
    {
        try
        {
            IsBusy = true;
            var data = Expenditure;

            var items = await _expenseHeadService.FilteredHeaders("");

            if (ExpenseHeads.Count != 0)
                ExpenseHeads.Clear();

            foreach (var item in items)
            {
                ExpenseHeads.Add(item);
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

    public void SetExpenseHead()
    {
        if (ExpenseHeads.Count != 0 && Expenditure.ExpenditureHeadId > 0)
        {
            SelectedExpenseHead = ExpenseHeads.FirstOrDefault(h => h.Id == Expenditure.ExpenditureHeadId) ?? new();
        }
        else
        {
            SelectedExpenseHead = new();
        }
    }

}
