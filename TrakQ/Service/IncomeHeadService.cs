using Microsoft.EntityFrameworkCore;
using TrakQ.Db;
using TrakQ.Db.Data.Entities;

namespace TrakQ.Service;
public sealed class IncomeHeadService
{
    private readonly IDbContextFactory<AppDbContext> _contextFactory;

    public IncomeHeadService(IDbContextFactory<AppDbContext> contextFactory)
    {
        _contextFactory = contextFactory;
    }

    public async Task<List<IncomeHead>> GetIncomeHeadsAsync()
    {
        using var _dbContext = await _contextFactory.CreateDbContextAsync();
        return await _dbContext.IncomeHeads
            .OrderBy(a => a.IncomeHeadName)
            .ToListAsync();
    }

    public async Task<bool> CreateIncomeHeadAsync(IncomeHead data)
    {
        using var _dbContext = await _contextFactory.CreateDbContextAsync();
        var entity = new IncomeHead
        {
            IncomeHeadName = data.IncomeHeadName,
            Note = data.Note
        };

        _dbContext.IncomeHeads.Add(entity);
        await _dbContext.SaveChangesAsync();
        return true;
    }

    public async Task<bool> UpdateIncomeHeadAsync(IncomeHead data)
    {
        using var _dbContext = await _contextFactory.CreateDbContextAsync();
        var incomeHead = await _dbContext.IncomeHeads
                            .FirstOrDefaultAsync(a => a.IncomeHeadId == data.IncomeHeadId);

        if (incomeHead is null)
        {
            throw new Exception("Invalid income head");
        }

        incomeHead.IncomeHeadName = data.IncomeHeadName;
        incomeHead.Note = data.Note;

        await _dbContext.SaveChangesAsync();
        return true;
    }
}
