using TrakQ.Db.Data.Entities;
using TrakQ.Service;
using TrakQ.View;

namespace TrakQ.ViewModel;
public partial class IncomeHeadViewModel : BaseViewModel
{
    private readonly IncomeHeadService _incomeHeadService;
    public ObservableCollection<IncomeHead> IncomeHeads { get; set; } = [];



    public IncomeHeadViewModel(IncomeHeadService incomeHeadService)
    {
        Title = "Income Heads";
        _incomeHeadService = incomeHeadService;
    }


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
            IsRefreshing = false;
        }




        var filename = "trakq_" + DateTime.Now.ToString("dd_mm_yyyy_HH_mm_ss") + ".txt";
        var ApplicationDataDirectory = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        var LocalApplicationDataDirectory = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

        // Does not work in android
        var CommonDocumentsDirectory = Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments);
        
        // Does not work in android and windows.
        var CommonProgramFilesDirectory = Environment.GetFolderPath(Environment.SpecialFolder.CommonProgramFiles);

        
        try
        {
            if (!Directory.Exists(ApplicationDataDirectory))
            {
                await Shell.Current.DisplayAlert("No directory-1", "ApplicationDataDirectory", "OK");
            }

            // Test-Datei schreiben
            File.WriteAllText(Path.Combine(ApplicationDataDirectory, "trakq", filename), "some text ApplicationDataDirectory");
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("ERROR-1", ex.Message, "OK");
        }


        try
        {
            if (!Directory.Exists(LocalApplicationDataDirectory))
            {
                await Shell.Current.DisplayAlert("No directory-2", "LocalApplicationDataDirectory", "OK");
            }

            // Test-Datei schreiben
            File.WriteAllText(Path.Combine(LocalApplicationDataDirectory, "trakq", filename), "some text LocalApplicationDataDirectory");
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("ERROR-2", ex.Message, "OK");
        }


        // Not working in androuif
        try
        {
            if (!Directory.Exists(CommonDocumentsDirectory))
            {
                await Shell.Current.DisplayAlert("No directory-3", "CommonDocumentsDirectory", "OK");
            }

            // Test-Datei schreiben
            File.WriteAllText(Path.Combine(CommonDocumentsDirectory, "trakq", filename), "some text CommonDocumentsDirectory");
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("ERROR-3", ex.Message, "OK");
        }

    }


    [RelayCommand]
    async Task CreateNew()
    {
        await GoToDetails(null);
    }

    [RelayCommand]
    async Task GoToDetails(IncomeHead? item)
    {
        if (item == null)
            item = new IncomeHead();

        await Shell.Current.GoToAsync(nameof(IncomeHeadFormPage), true, new Dictionary<string, object>
        {
            {"IncomeHead", item }
        });
    }
}
