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

    [RelayCommand]
    async Task PickFile()
    {
        if (IsBusy)
            return;


        var customFileType = new FilePickerFileType(
                   new Dictionary<DevicePlatform, IEnumerable<string>>
                   {
                       { DevicePlatform.iOS, ["public.text"] }, // UTType values  
                       { DevicePlatform.Android, ["text/plain"] }, // MIME type  
                       { DevicePlatform.WinUI, [".txt"] }, // file extension  
                       { DevicePlatform.macOS, ["txt"] },
                   });

        PickOptions pickOptions = new()
        {
            PickerTitle = "Pick sql file",
            FileTypes = customFileType
        };        

        try
        {
            IsBusy = true;
            var sqlFile = await FilePicker.Default.PickAsync(pickOptions);

            if (sqlFile is not null)
            {
                var fileStream = await sqlFile.OpenReadAsync();
                using var reader = new StreamReader(fileStream);
                var rawQuery = await reader.ReadToEndAsync();

                if (!string.IsNullOrEmpty(rawQuery))
                {
                    var formattedQuery = FormattableStringFactory.Create(rawQuery);
                    await _expenseHeadService.ImportBulkDataAsync(formattedQuery);
                }
            }            
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Unable to save income: {ex.StackTrace}");
            await Shell.Current.DisplayAlert("Error!", ex.Message + ex.StackTrace, "OK");
        }
        finally
        {
            IsBusy = false;
            IsRefreshing = false;
        }
    }
}
