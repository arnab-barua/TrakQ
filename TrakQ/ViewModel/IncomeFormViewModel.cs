using TrakQ.Db.Data.Entities;
using TrakQ.Dto;
using TrakQ.Service;

namespace TrakQ.ViewModel;

[QueryProperty(nameof(Income), "Income")]
public partial class IncomeFormViewModel : BaseViewModel
{
    private readonly IncomeService _incomeService;
    private readonly IncomeHeadService _incomeHeadService;

    public ObservableCollection<IncomeHead> IncomeHeads { get; set; } = [];

    public IncomeFormViewModel(IncomeService incomeService, IncomeHeadService incomeHeadService)
    {
        _incomeService = incomeService;
        _incomeHeadService = incomeHeadService;
    }

    [ObservableProperty]
    IncomeViewDto income = new();

    [ObservableProperty]
    IncomeHead selectedIncomeHead = new();

    [RelayCommand]
    async Task SaveAsync()
    {
        if (IsBusy)
            return;

        try
        {
            IsBusy = true;
            var selectedIncomeHd = selectedIncomeHead;
            var data = income;

            if(SelectedIncomeHead?.IncomeHeadId == null)
            {
                await Shell.Current.DisplayAlert("Error!", "Select Income head", "OK");
                return;
            }

            income.IncomeHeadId = SelectedIncomeHead.IncomeHeadId;
            income.IncomeDate = Income.IncomeDate.AddHours(12);

            int changedId = 0;
            if (income.IncomeId == 0)
            {
                changedId = await _incomeService.AddAsync(income);
            }
            else
            {
                changedId = await _incomeService.UpdateAsync(income.IncomeId, income);
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
    async Task GetAllIncomeHeadsAsync()
    {
        try
        {
            var items = await _incomeHeadService.GetIncomeHeadsAsync();

            if (IncomeHeads.Count != 0)
                IncomeHeads.Clear();

            foreach (var item in items)
            {
                IncomeHeads.Add(item);
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

    public void SetIncomeHead()
    {
        if(IncomeHeads.Count != 0 && income.IncomeHeadId > 0)
        {
            SelectedIncomeHead = IncomeHeads.FirstOrDefault(h => h.IncomeHeadId == income.IncomeHeadId);
        }
    }

}
