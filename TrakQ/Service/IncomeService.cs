using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrakQ.Db;
using TrakQ.Db.Data.Entities;
using TrakQ.Dto;

namespace TrakQ.Service;
public sealed class IncomeService
{
    private readonly IDbContextFactory<AppDbContext> _contextFactory;

    private static readonly Func<AppDbContext, DateTime, DateTime, IAsyncEnumerable<IncomeViewDto>> _getMonthDataQuery =
        EF.CompileAsyncQuery((AppDbContext context, DateTime start, DateTime end) =>
            context.Incomes
                .Where(a => a.IncomeDate >= start && a.IncomeDate < end && !a.IsDeleted)
                .OrderBy(a => a.IncomeDate.Date)
                .Select(a => new IncomeViewDto
                {
                    IncomeId = a.IncomeId,
                    IncomeHeadId = a.IncomeHeadId,
                    IncomeHeadName = a.IncomeHead.IncomeHeadName,
                    IncomeDate = a.IncomeDate,
                    Amount = a.Amount,
                    Remark = a.Remark
                }));

    private static readonly Func<AppDbContext, DateTime, DateTime, Task<decimal>> _getTotalMonthIncomeQuery =
        EF.CompileAsyncQuery((AppDbContext context, DateTime start, DateTime end) =>
            context.Incomes
                .Where(a => a.IncomeDate >= start && a.IncomeDate < end && !a.IsDeleted)
                .Sum(a => a.Amount));

    public IncomeService(IDbContextFactory<AppDbContext> contextFactory)
    {
        _contextFactory = contextFactory;
    }

    public async Task<List<IncomeViewDto>> GetMonthDataAsync(int year, int month)
    {
        using var _dbContext = await _contextFactory.CreateDbContextAsync();
        DateTime start = new(year, month, 1, 0, 0, 0);
        DateTime end = start.AddMonths(1);

        var list = new List<IncomeViewDto>();
        await foreach (var item in _getMonthDataQuery(_dbContext, start, end))
        {
            list.Add(item);
        }
        return list;
    }

    public async Task<decimal> GetTotalMonthIncomeAsync(int year, int month)
    {
        using var _dbContext = await _contextFactory.CreateDbContextAsync();
        DateTime start = new(year, month, 1, 0, 0, 0);
        DateTime end = start.AddMonths(1);

        return await _getTotalMonthIncomeQuery(_dbContext, start, end);
    }

    public async Task<int> AddAsync(IncomeViewDto formDto)
    {
        using var _dbContext = await _contextFactory.CreateDbContextAsync();
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
        using var _dbContext = await _contextFactory.CreateDbContextAsync();
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
