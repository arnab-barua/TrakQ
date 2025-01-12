using Microsoft.EntityFrameworkCore;
using TrakQ.Db.Data.Entities;

namespace TrakQ.Db;
public class AppDbContext : DbContext
{

    public DbSet<ExpenditureHead> ExpenditureHeads { get; set; }
    public DbSet<Expenditure> Expenditures { get; set; }
    public DbSet<IncomeHead> IncomeHeads { get; set; }
    public DbSet<Income> Incomes { get; set; }


    public AppDbContext()
    {
        
    }


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var dbPath = Path.Combine(Constants.DatabasePath);

        optionsBuilder.UseSqlite($"Filename = {dbPath}");
        base.OnConfiguring(optionsBuilder);
    }
}
