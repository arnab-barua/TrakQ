using Microsoft.EntityFrameworkCore;
using TrakQ.Db;
using TrakQ.Db.Data.Entities;

namespace TrakQ.Service;

public class FiscalMonthService
{
    private readonly AppDbContext _context;

    public FiscalMonthService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<FiscalMonth?> GetFiscalMonth(int year, int month)
    {
        return await _context.FiscalMonths
            .Where(a => a.Year == year && a.Month == month)
            .AsNoTracking()
            .FirstOrDefaultAsync();
    }
}
