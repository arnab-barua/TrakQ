using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrakQ.Db.Data.Entities;
using TrakQ.Service;

namespace TrakQ.ViewModel;

[QueryProperty(nameof(IncomeHead), "IncomeHead")]
public partial class IncomeHeadFormViewModel : BaseViewModel
{
    private readonly IncomeHeadService _incomeHeadService;

    public IncomeHeadFormViewModel(IncomeHeadService incomeHeadService)
    {
        Title = "Add/update income head";
        _incomeHeadService = incomeHeadService;
    }

    [ObservableProperty]
    IncomeHead incomeHead = new();

    [RelayCommand]
    async Task SaveAsync()
    {
        if (IsBusy)
            return;

        try
        {
            IsBusy = true;

            bool isSuccess;
            if (incomeHead.IncomeHeadId == 0)
            {
                isSuccess = await _incomeHeadService.CreateIncomeHeadAsync(incomeHead);
            }
            else
            {
                isSuccess = await _incomeHeadService.UpdateIncomeHeadAsync(incomeHead);
            }

            if (isSuccess)
            {
                await Shell.Current.GoToAsync(".."); // Navigate back
            }

        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Unable to get monkeys: {ex.Message}");
            await Shell.Current.DisplayAlert("Error!", ex.Message, "OK");
        }
        finally
        {
            IsBusy = false;
        }

    }

}
