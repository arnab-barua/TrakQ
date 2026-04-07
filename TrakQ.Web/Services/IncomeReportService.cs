using Microsoft.EntityFrameworkCore;
using TrakQ.Db;
using TrakQ.Web.Models;

namespace TrakQ.Web.Services;

public class IncomeReportService : IIncomeReportService
{
    private readonly IDbContextFactory<AppDbContext> _dbFactory;

    public IncomeReportService(IDbContextFactory<AppDbContext> dbFactory)
        => _dbFactory = dbFactory;

    public async Task<List<IncomeHeadGroup>> GetMonthlyIncomeByHeadAsync(int year, int month)
    {
        await using var db = _dbFactory.CreateDbContext();

        var start = new DateTime(year, month, 1);
        var end = start.AddMonths(1);

        var rows = await db.Incomes
            .Where(i => i.IncomeDate >= start && i.IncomeDate < end && !i.IsDeleted)
            .Include(i => i.IncomeHead)
            .OrderBy(i => i.IncomeDate)
            .AsNoTracking()
            .ToListAsync();

        return rows
            .GroupBy(i => i.IncomeHead.IncomeHeadName)
            .Select(g => new IncomeHeadGroup
            {
                HeadName = g.Key,
                Rows = g.Select(i => new IncomeRow(i.IncomeDate, i.Amount, i.Remark)).ToList()
            })
            .OrderBy(g => g.HeadName)
            .ToList();
    }

    public async Task<List<MonthlyTotals>> GetMonthlyIncomeTotalsAsync(int monthsBack)
    {
        await using var db = _dbFactory.CreateDbContext();

        var cutoff = DateTime.Now.AddMonths(-monthsBack + 1);
        var start = new DateTime(cutoff.Year, cutoff.Month, 1);

        var rows = await db.Incomes
            .Where(i => i.IncomeDate >= start && !i.IsDeleted)
            .AsNoTracking()
            .ToListAsync();

        return rows
            .GroupBy(i => new { i.IncomeDate.Year, i.IncomeDate.Month })
            .Select(g => new MonthlyTotals(g.Key.Year, g.Key.Month, g.Sum(i => i.Amount), 0))
            .OrderBy(t => t.Year).ThenBy(t => t.Month)
            .ToList();
    }
}
