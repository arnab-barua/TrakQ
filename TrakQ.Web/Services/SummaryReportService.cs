using Microsoft.EntityFrameworkCore;
using TrakQ.Db;
using TrakQ.Web.Models;

namespace TrakQ.Web.Services;

public class SummaryReportService : ISummaryReportService
{
    private readonly IDbContextFactory<AppDbContext> _dbFactory;

    public SummaryReportService(IDbContextFactory<AppDbContext> dbFactory)
        => _dbFactory = dbFactory;

    public async Task<MonthlySummaryData> GetSummaryAsync(int year, int month)
    {
        await using var db = _dbFactory.CreateDbContext();

        var start = new DateTime(year, month, 1);
        var end = start.AddMonths(1);

        // Total income
        var incomes = await db.Incomes
            .Where(i => i.IncomeDate >= start && i.IncomeDate < end && !i.IsDeleted)
            .AsNoTracking()
            .ToListAsync();
        var totalIncome = incomes.Sum(i => i.Amount);

        // Total expense + top heads
        var expenditures = await db.Expenditures
            .Where(e => e.ExpenditureDate >= start && e.ExpenditureDate < end && !e.IsDeleted)
            .Include(e => e.ExpenditureHead)
            .AsNoTracking()
            .ToListAsync();
        var totalExpense = expenditures.Sum(e => e.Amount);

        var topHeads = expenditures
            .GroupBy(e => e.ExpenditureHead.HeadName)
            .Select(g => new ExpenseHeadShare(g.Key, g.Sum(e => e.Amount)))
            .OrderByDescending(s => s.Amount)
            .Take(5)
            .ToList();

        // Account balances via FiscalMonth join
        var fiscalMonth = await db.FiscalMonths
            .AsNoTracking()
            .FirstOrDefaultAsync(f => f.Year == year && f.Month == month);

        decimal totalOpening = 0, totalClosing = 0;
        if (fiscalMonth != null)
        {
            var sheets = await db.AccountSheets
                .Where(s => s.FiscalMonthId == fiscalMonth.Id)
                .AsNoTracking()
                .ToListAsync();

            totalOpening = sheets.Sum(s => s.OpeningBalance);
            totalClosing = sheets.Sum(s => s.ClosingBalance ?? 0);
        }

        return new MonthlySummaryData
        {
            Year = year,
            Month = month,
            TotalIncome = totalIncome,
            TotalExpense = totalExpense,
            TotalOpeningBalance = totalOpening,
            TotalClosingBalance = totalClosing,
            TopExpenseHeads = topHeads
        };
    }

    public async Task<List<MonthlyTotals>> GetTrendAsync(int monthsBack)
    {
        await using var db = _dbFactory.CreateDbContext();

        var cutoff = DateTime.Now.AddMonths(-monthsBack + 1);
        var start = new DateTime(cutoff.Year, cutoff.Month, 1);

        var incomes = await db.Incomes
            .Where(i => i.IncomeDate >= start && !i.IsDeleted)
            .AsNoTracking()
            .ToListAsync();

        var expenses = await db.Expenditures
            .Where(e => e.ExpenditureDate >= start && !e.IsDeleted)
            .AsNoTracking()
            .ToListAsync();

        var incomeLookup = incomes
            .GroupBy(i => new { i.IncomeDate.Year, i.IncomeDate.Month })
            .ToDictionary(g => (g.Key.Year, g.Key.Month), g => g.Sum(i => i.Amount));

        var expenseLookup = expenses
            .GroupBy(e => new { e.ExpenditureDate.Year, e.ExpenditureDate.Month })
            .ToDictionary(g => (g.Key.Year, g.Key.Month), g => g.Sum(e => e.Amount));

        // Build a list covering all months that have any data
        var allKeys = incomeLookup.Keys.Union(expenseLookup.Keys).Distinct()
            .OrderBy(k => k.Year).ThenBy(k => k.Month)
            .ToList();

        return allKeys.Select(k => new MonthlyTotals(
            k.Year,
            k.Month,
            incomeLookup.TryGetValue(k, out var inc) ? inc : 0,
            expenseLookup.TryGetValue(k, out var exp) ? exp : 0
        )).ToList();
    }
}
