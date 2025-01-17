using Microsoft.EntityFrameworkCore;
using TrakQ.Db;
using TrakQ.Db.Data.Entities;
using TrakQ.Dto;

namespace TrakQ.Service;
public sealed class IncomeService
{
    private readonly AppDbContext _dbContext;

    public IncomeService(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<IncomeViewDto>> GetMonthDataAsync(int year, int month)
    {
        DateTime start = new(year, month, 1, 0, 0, 0);
        DateTime end = start.AddMonths(1);

        return await _dbContext.Incomes
                            .Where(a => a.IncomeDate >= start
                                    && a.IncomeDate < end
                                    && !a.IsDeleted)
                            .Include(a => a.IncomeHead)
                            .OrderBy(a => a.IncomeDate.Date)
                            .AsNoTracking()
                            .Select(a => new IncomeViewDto
                            {
                                IncomeId = a.IncomeId,
                                IncomeHeadId = a.IncomeHeadId,
                                IncomeHeadName = a.IncomeHead.IncomeHeadName,
                                IncomeDate = a.IncomeDate,
                                Amount = a.Amount,
                                Remark = a.Remark
                            })
                            .ToListAsync();
    }


    public async Task<decimal> GetTotalMonthIncomeAsync(int year, int month)
    {
        DateTime start = new(year, month, 1, 0, 0, 0);
        DateTime end = start.AddMonths(1);

        return await _dbContext.Incomes
                            .Where(a => a.IncomeDate >= start
                                    && a.IncomeDate < end
                                    && !a.IsDeleted)
                            .SumAsync(a => a.Amount);
    }

    public async Task<int> AddAsync(IncomeViewDto formDto)
    {
        var incomeHead = await _dbContext.IncomeHeads
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.IncomeHeadId == formDto.IncomeHeadId);

        if (incomeHead is null)
        {
            throw new Exception("Income head not found!");
        }

        var entity = new Income
        {
            IncomeHeadId = formDto.IncomeHeadId,
            Amount = formDto.Amount,
            Remark = formDto.Remark,
            IncomeDate = formDto.IncomeDate,
            IsDeleted = false
        };


        _dbContext.Incomes.Add(entity);
        await _dbContext.SaveChangesAsync();

        return entity.IncomeId;
    }

    public async Task<int> UpdateAsync(int id, IncomeViewDto formDto)
    {
        var income = await _dbContext.Incomes
                            .FirstOrDefaultAsync(a => a.IncomeId == id);

        if (income is null)
        {
            throw new Exception("Income not found!");
        }


        if (income.IncomeHeadId != formDto.IncomeHeadId)
        {
            var incomeHead = await _dbContext.IncomeHeads
                            .Where(a => a.IncomeHeadId == formDto.IncomeHeadId)
                            .AsNoTracking()
                            .FirstOrDefaultAsync();

            if (incomeHead is null)
            {
                throw new Exception("Income head not found!");
            }

            income.IncomeHeadId = formDto.IncomeHeadId;
        }


        income.IncomeDate = formDto.IncomeDate;
        income.Amount = formDto.Amount;
        income.Remark = formDto.Remark;

        await _dbContext.SaveChangesAsync();

        return id;
    }
}
