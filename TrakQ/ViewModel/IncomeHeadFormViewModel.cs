using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TrakQ.Db.Data.Entities;
using TrakQ.Service;

namespace TrakQ.ViewModel;

internal class IncomeHeadFormViewModel : BindableObject
{
    private readonly IncomeHeadService _incomeHeadService;   

    public string PageTitle { get; set; }
    public IncomeHead CurrentIncomeHead { get; set; }

    public ICommand SaveCommand { get; }


    public IncomeHeadFormViewModel(IncomeHeadService incomeHeadService)
    {
        _incomeHeadService = incomeHeadService;
        SaveCommand = new Command(async () => await SaveAsync());
    }


    public async Task InitializeAsync(IncomeHead? incomeHead)
    {
        if (incomeHead == null)
        {
            PageTitle = "Create Expense Head";
            CurrentIncomeHead = new IncomeHead();
        }
        else
        {
            PageTitle = "Edit Expense Head";
            CurrentIncomeHead = incomeHead;
        }

        OnPropertyChanged(nameof(PageTitle));
        OnPropertyChanged(nameof(CurrentIncomeHead));
    }

    private async Task SaveAsync()
    {
        bool isSuccess;
        if (CurrentIncomeHead.IncomeHeadId == 0)
        {
            isSuccess = await _incomeHeadService.CreateIncomeHeadAsync(CurrentIncomeHead);
        }
        else
        {
            isSuccess = await _incomeHeadService.UpdateIncomeHeadAsync(CurrentIncomeHead);
        }

        if (isSuccess)
        {
            await Shell.Current.GoToAsync(".."); // Navigate back
        }
        else
        {
            await Application.Current.MainPage.DisplayAlert("Error", "Failed to save the expense head.", "OK");
        }
    }
}
