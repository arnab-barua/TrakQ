using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TrakQ.Dto;
using TrakQ.Service;

namespace TrakQ.ViewModel;
internal class ExpenseHeadFormViewModel : BindableObject
{
    private readonly ExpenseHeadService _expenseHeadService;

    public string PageTitle { get; set; }
    public ExpenseHeadDto CurrentExpenseHead { get; set; }

    public ICommand SaveCommand { get; }

    public ExpenseHeadFormViewModel(ExpenseHeadService expenseHeadService)
    {
        _expenseHeadService = expenseHeadService;
        SaveCommand = new Command(async () => await SaveAsync());
    }

    public async Task InitializeAsync(ExpenseHeadDto? expenseHead)
    {
        if (expenseHead == null)
        {
            PageTitle = "Create Expense Head";
            CurrentExpenseHead = new ExpenseHeadDto();
        }
        else
        {
            PageTitle = "Edit Expense Head";
            CurrentExpenseHead = expenseHead;
        }

        OnPropertyChanged(nameof(PageTitle));
        OnPropertyChanged(nameof(CurrentExpenseHead));
    }

    private async Task SaveAsync()
    {
        bool isSuccess;
        if (CurrentExpenseHead.Id == 0)
        {
            isSuccess = await _expenseHeadService.CreateExpenseHeadAsync(CurrentExpenseHead);
        }
        else
        {
            isSuccess = await _expenseHeadService.UpdateExpenseHeadAsync(CurrentExpenseHead);
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