using Microsoft.Maui.Controls;
using TrakQ.ViewModel;

namespace TrakQ.View;

public partial class LogsPage : ContentPage
{
    private readonly LogsPageViewModel _viewModel;

    public LogsPage(LogsPageViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.LoadLogsAsync();
    }
}

