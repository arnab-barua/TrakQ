using Microsoft.EntityFrameworkCore;
using TrakQ.Db;
using TrakQ.Db.Data.Entities;

namespace TrakQ.Service;

public class FiscalMonthService
{
    private readonly IDbContextFactory<AppDbContext> _contextFactory;

    public FiscalMonthService(IDbContextFactory<AppDbContext> contextFactory)
    {
        _contextFactory = contextFactory;
    }

    public async Task<FiscalMonth?> GetFiscalMonth(int year, int month)
    {
        using var _context = await _contextFactory.CreateDbContextAsync();
        return await _context.FiscalMonths
            .Where(a => a.Year == year && a.Month == month)
            .AsNoTracking()
            .FirstOrDefaultAsync();
    }
}
