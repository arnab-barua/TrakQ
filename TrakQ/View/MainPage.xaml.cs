using TrakQ.ViewModel;

namespace TrakQ.View;

public partial class MainPage : ContentPage
{
    public MainPage(MainPageViewModel viewModel)
    {        
        InitializeComponent();
        BindingContext = viewModel;
    }
}
