using Microsoft.EntityFrameworkCore;
using TrakQ.Db;

namespace TrakQ
{
    public partial class App : Application
    {
        public App(AppDbContext context)
        {
            InitializeComponent();
            context.Database.Migrate();
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new AppShell());
        }
    }
}