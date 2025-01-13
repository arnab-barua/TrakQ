using TrakQ.Db.Data.Entities;

namespace TrakQ.ViewModel;
public partial class MainPageViewModel : BaseViewModel
{
    public MainPageViewModel()
    {
        Title = "Dashboard";
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
}
