using Microsoft.EntityFrameworkCore;
using TrakQ.Db;

namespace TrakQ
{
    public partial class App : Application
    {
        
        private readonly TrakQ.Service.ExceptionLoggerService _logger;

        public App(AppDbContext context, TrakQ.Service.ExceptionLoggerService logger)
        {
            InitializeComponent();
            _logger = logger;
            if(!Directory.Exists(Constants.ApplicationPath))
            {
                Directory.CreateDirectory(Constants.ApplicationPath);
            }
            context.Database.Migrate();

            // Wire up global exception handlers
            AppDomain.CurrentDomain.UnhandledException += (s, e) => 
            {
                if (e.ExceptionObject is Exception ex)
                {
                    _logger.LogException(ex);
                }
            };

            TaskScheduler.UnobservedTaskException += (s, e) =>
            {
                if (e.Exception is Exception ex)
                {
                    _logger.LogException(ex);
                }
                e.SetObserved(); // Prevent app crash for unobserved task exceptions
            };
        }


        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new AppShell());
        }
    }
}
