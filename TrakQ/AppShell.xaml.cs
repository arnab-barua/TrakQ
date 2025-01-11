using TrakQ.View;

namespace TrakQ
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(IncomeHeadPage), typeof(IncomeHeadPage));
            Routing.RegisterRoute(nameof(IncomeHeadFormPage), typeof(IncomeHeadFormPage));
            Routing.RegisterRoute(nameof(IncomePage), typeof(IncomePage));
            Routing.RegisterRoute(nameof(IncomeFormPage), typeof(IncomeFormPage));
        }
    }
}
