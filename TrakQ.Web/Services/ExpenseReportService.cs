using Microsoft.EntityFrameworkCore;
using TrakQ.Db;
using TrakQ.Web.Models;

namespace TrakQ.Web.Services;

public class ExpenseReportService : IExpenseReportService
{
    private readonly IDbContextFactory<AppDbContext> _dbFactory;

    public ExpenseReportService(IDbContextFactory<AppDbContext> dbFactory)
        => _dbFactory = dbFactory;

    public async Task<List<DailyExpenseRow>> GetMonthlyExpensesAsync(int year, int month)
    {
        await using var db = _dbFactory.CreateDbContext();

        var start = new DateTime(year, month, 1);
        var end = start.AddMonths(1);

        return await db.Expenditures
            .Where(e => e.ExpenditureDate >= start && e.ExpenditureDate < end && !e.IsDeleted)
            .Include(e => e.ExpenditureHead)
            .OrderBy(e => e.ExpenditureDate)
            .AsNoTracking()
            .Select(e => new DailyExpenseRow(
                e.ExpenditureDate,
                e.ExpenditureHead.HeadName,
                e.Amount,
                e.Remark))
            .ToListAsync();
    }

    public async Task<List<ExpenseHeadTotalNode>> GetExpenseByHeadAsync(int year, int month)
    {
        await using var db = _dbFactory.CreateDbContext();

        var start = new DateTime(year, month, 1);
        var end = start.AddMonths(1);

        // Load all heads and expenditures for the period
        var allHeads = await db.ExpenditureHeads
            .AsNoTracking()
            .OrderBy(h => h.ExpenditureHeadId)
            .ToListAsync();

        var expenditures = await db.Expenditures
            .Where(e => e.ExpenditureDate >= start && e.ExpenditureDate < end && !e.IsDeleted)
            .AsNoTracking()
            .ToListAsync();

        // Group expenditure amounts by head
        var totalsByHead = expenditures
            .GroupBy(e => e.ExpenditureHeadId)
            .ToDictionary(g => g.Key, g => g.Sum(e => e.Amount));

        // Build node map
        var nodeMap = allHeads.ToDictionary(h => h.ExpenditureHeadId, h => new ExpenseHeadTotalNode
        {
            HeadId = h.ExpenditureHeadId,
            HeadName = h.HeadName,
            ParentHeadId = h.ParentHeadId,
            Budget = h.Budget,
            Total = totalsByHead.TryGetValue(h.ExpenditureHeadId, out var t) ? t : 0m
        });

        // Assign children
        foreach (var node in nodeMap.Values)
        {
            if (node.ParentHeadId != 0 && nodeMap.TryGetValue(node.ParentHeadId, out var parent))
                parent.Children.Add(node);
        }

        // Roll up totals bottom-up (leaves first via reverse topological order)
        var roots = nodeMap.Values.Where(n => n.ParentHeadId == 0).ToList();
        RollUpTotals(roots);

        return roots;
    }

    private static void RollUpTotals(List<ExpenseHeadTotalNode> nodes)
    {
        foreach (var node in nodes)
        {
            RollUpTotals(node.Children);
            node.Total += node.Children.Sum(c => c.Total);
        }
    }
}
