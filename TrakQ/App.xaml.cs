using Microsoft.EntityFrameworkCore;
using TrakQ.Db;

namespace TrakQ
{
    public partial class App : Application
    {
        private readonly TrakQ.Service.ExceptionLoggerService _logger;
        public static Exception? StartupException { get; set; }

        public App(IDbContextFactory<AppDbContext> dbContextFactory, TrakQ.Service.ExceptionLoggerService logger)
        {
            _logger = logger;

            // Wire up global exception handlers immediately
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
                e.SetObserved();
            };

            InitializeComponent();

            // Ensure database folder exists safely
            try
            {
                if (!Directory.Exists(Constants.ApplicationPath))
                {
                    Directory.CreateDirectory(Constants.ApplicationPath);
                }
            }
            catch (Exception ex)
            {
                StartupException = ex;
                _logger.LogException(ex);
            }

            // Initialize database in background: sets WAL mode and runs migrations without freezing the splash screen
            Task.Run(async () =>
            {
                try
                {
                    using var context = await dbContextFactory.CreateDbContextAsync();
                    
                    // Enable WAL mode, busy timeout (5s), and safe synchronous mode
                    await context.Database.ExecuteSqlRawAsync("PRAGMA journal_mode = WAL; PRAGMA busy_timeout = 5000; PRAGMA synchronous = NORMAL;");

                    // Run pending migrations safely
                    await context.Database.MigrateAsync();
                }
                catch (Exception ex)
                {
                    StartupException = ex;
                    _logger.LogException(ex);
                }
            });
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new AppShell());
        }
    }
}
