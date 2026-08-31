using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.ApplicationModel.DataTransfer;
using TrakQ.Service;

namespace TrakQ.ViewModel;

public partial class LogsPageViewModel : BaseViewModel
{
    private readonly ExceptionLoggerService _loggerService;

    [ObservableProperty]
    private string logText = string.Empty;

    public LogsPageViewModel(ExceptionLoggerService loggerService)
    {
        Title = "System Logs";
        _loggerService = loggerService;
    }

    [RelayCommand]
    public async Task LoadLogsAsync()
    {
        IsBusy = true;
        LogText = await _loggerService.GetLogsAsync();
        IsBusy = false;
    }

    [RelayCommand]
    public async Task ShareLogsAsync()
    {
        var filePath = _loggerService.GetLogFilePath();
        if (System.IO.File.Exists(filePath))
        {
            await Share.Default.RequestAsync(new ShareFileRequest
            {
                Title = "Share TrakQ Error Logs",
                File = new ShareFile(filePath)
            });
        }
        else
        {
            await Shell.Current.DisplayAlert("Info", "No log file exists to share.", "OK");
        }
    }

    [RelayCommand]
    public void ClearLogs()
    {
        _loggerService.ClearLogs();
        LogText = "No logs found.";
    }
}

