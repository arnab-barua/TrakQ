using TrakQ.ViewModel;

namespace TrakQ.View;

public partial class MainPage : ContentPage
{
    public MainPage(MainPageViewModel viewModel)
    {        
        InitializeComponent();
        BindingContext = viewModel;
        Appearing += OnAppearing;
    }

    private async void OnAppearing(object? sender, EventArgs e)
    {
        if (App.StartupException is not null)
        {
            var ex = App.StartupException;
            App.StartupException = null; // Alert only once
            await DisplayAlert(
                "Database Notice",
                $"A database startup issue was detected:\n\n{ex.Message}\n\nThe app is running, but you may want to check 'System Logs -> Error Logs' or restore a backup if your data does not load properly.",
                "OK");
        }
    }
}
