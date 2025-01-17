using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrakQ.Db.Data.Entities;
using TrakQ.Dto;
using TrakQ.Service;
using TrakQ.View;

namespace TrakQ.ViewModel;
public partial class IncomeViewModel : BaseViewModel
{
    private readonly IncomeService _incomeService;
    public ObservableCollection<IncomeViewDto> Incomes { get; set; } = [];



    public IncomeViewModel(IncomeService incomeService)
    {
        Title = "Incomes";
        _incomeService = incomeService;
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
            var items = await _incomeService.GetMonthDataAsync(Year, Month);

            if (Incomes.Count != 0)
                Incomes.Clear();

            foreach (var item in items)
            {
                Incomes.Add(item);
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
            IsRefreshing = false;
        }

    }


    [RelayCommand]
    async Task CreateNew()
    {
        await GoToDetails(null);
    }

    [RelayCommand]
    async Task GoToDetails(IncomeViewDto? item)
    {
        if (item == null)
            item = new IncomeViewDto();

        await Shell.Current.GoToAsync(nameof(IncomeFormPage), true, new Dictionary<string, object>
        {
            {"Income", item }
        });
    }
}
