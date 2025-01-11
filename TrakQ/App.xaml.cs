using Microsoft.EntityFrameworkCore;
using TrakQ.Db;

namespace TrakQ
{
    public partial class App : Application
    {
        //private readonly AppDbContext _context;
        public App(AppDbContext context)
        {
            InitializeComponent();
            //_context = context;
            context.Database.Migrate();
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new AppShell());
        }
    }
}