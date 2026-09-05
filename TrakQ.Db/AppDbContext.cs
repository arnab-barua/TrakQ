using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using TrakQ.Db.Data.Entities;

namespace TrakQ.Db;
public class AppDbContext : DbContext
{
    public DbSet<ExpenditureHead> ExpenditureHeads { get; set; }
    public DbSet<Expenditure> Expenditures { get; set; }
    public DbSet<FiscalMonth> FiscalMonths { get; set; }
    public DbSet<Account> Accounts { get; set; }
    public DbSet<AccountSheet> AccountSheets { get; set; }
    public DbSet<IncomeHead> IncomeHeads { get; set; }
    public DbSet<Income> Incomes { get; set; }

    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            var connectionStringBuilder = new SqliteConnectionStringBuilder
            {
                DataSource = Constants.DatabasePath,
                Mode = SqliteOpenMode.ReadWriteCreate,
                Cache = SqliteCacheMode.Shared,
                DefaultTimeout = 15
            };

            optionsBuilder.UseSqlite(connectionStringBuilder.ToString());
        }
        base.OnConfiguring(optionsBuilder);
    }
}
