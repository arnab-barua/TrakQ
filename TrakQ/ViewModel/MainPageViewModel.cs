using TrakQ.Db.Data.Entities;
using TrakQ.Service;

namespace TrakQ.ViewModel;
public partial class MainPageViewModel : BaseViewModel
{
    private readonly ImportExportService _importExportService;

    public MainPageViewModel(ImportExportService importExportService)
    {
        Title = "Dashboard";
        _importExportService = importExportService;
    }


    [RelayCommand]
    async Task RequestPermissionAsync()
    {
        try
        {
            if (DeviceInfo.Platform == DevicePlatform.Android)
            {
                // Storage write permission

                var status = PermissionStatus.Unknown;

                status = await Permissions.CheckStatusAsync<Permissions.StorageRead>()
                    .WaitAsync(new CancellationToken())
                    .ConfigureAwait(false);

                if (status != PermissionStatus.Granted)
                {
                    if (Permissions.ShouldShowRationale<Permissions.StorageRead>())
                    {
                        await Shell.Current.DisplayAlert("Needs permissions", "BECAUSE!!!", "OK");
                    }

                    status = await Permissions.RequestAsync<Permissions.StorageRead>();


                    if (status != PermissionStatus.Granted)
                        await Shell.Current.DisplayAlert("Permission required",
                            "Storage read pemission is required to import data", "OK");
                }



                // Storage write permission

                status = PermissionStatus.Unknown;

                status = await Permissions.CheckStatusAsync<Permissions.StorageWrite>();

                if (status != PermissionStatus.Granted)
                {
                    if (Permissions.ShouldShowRationale<Permissions.StorageWrite>())
                    {
                        await Shell.Current.DisplayAlert("Needs permissions", "BECAUSE!!!", "OK");
                    }

                    status = await Permissions.RequestAsync<Permissions.StorageWrite>();


                    if (status != PermissionStatus.Granted)
                        await Shell.Current.DisplayAlert("Permission required",
                            "Storage read pemission is required to import data", "OK");
                }
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Unable to get income heads: {ex.Message}");
            await Shell.Current.DisplayAlert("Error!", ex.Message + ex.StackTrace, "OK");
        }
        finally
        {

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
                    await _importExportService.ImportBulkDataAsync(rawQuery);
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
        }
    }
}
