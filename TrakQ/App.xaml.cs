using Microsoft.EntityFrameworkCore;
using TrakQ.Db;

namespace TrakQ
{
    public partial class App : Application
    {
        public App(AppDbContext context)
        {
            InitializeComponent();
            if(!Directory.Exists(Constants.ApplicationPath))
            {
                Directory.CreateDirectory(Constants.ApplicationPath);
            }
            context.Database.Migrate();
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new AppShell());
        }
    }
}