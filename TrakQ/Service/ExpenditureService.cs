using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrakQ.Db;
using TrakQ.Db.Data.Entities;
using TrakQ.Dto;

namespace TrakQ.Service;

public sealed class ExpenditureService
{
    private readonly IDbContextFactory<AppDbContext> _contextFactory;

    private static readonly Func<AppDbContext, DateTime, DateTime, IAsyncEnumerable<ExpenditureDto>> _getMonthDataQuery =
        EF.CompileAsyncQuery((AppDbContext context, DateTime start, DateTime end) =>
            context.Expenditures
                .Where(a => a.ExpenditureDate >= start && a.ExpenditureDate < end && !a.IsDeleted)
                .OrderBy(a => a.ExpenditureDate.Date)
                .Select(a => new ExpenditureDto
                {
                    ExpenditureId = a.ExpenditureId,
                    ExpenditureHeadId = a.ExpenditureHeadId,
                    ExpenditureHeadText = a.ExpenditureHead.HeadName,
                    ExpenditureDate = a.ExpenditureDate,
                    Amount = a.Amount,
                    Remark = a.Remark
                }));

    private static readonly Func<AppDbContext, DateTime, DateTime, Task<decimal>> _getTotalMonthExpenditureQuery =
        EF.CompileAsyncQuery((AppDbContext context, DateTime start, DateTime end) =>
            context.Expenditures
                .Where(a => a.ExpenditureDate >= start && a.ExpenditureDate < end && !a.IsDeleted)
                .Sum(a => a.Amount));

    public ExpenditureService(IDbContextFactory<AppDbContext> contextFactory)
    {
        _contextFactory = contextFactory;
    }

    public async Task<List<ExpenditureDto>> GetMonthDataAsync(int year, int month)
    {
        using var _context = await _contextFactory.CreateDbContextAsync();
        DateTime start = new(year, month, 1, 0, 0, 0);
        DateTime end = start.AddMonths(1);

        var list = new List<ExpenditureDto>();
        await foreach (var item in _getMonthDataQuery(_context, start, end))
        {
            list.Add(item);
        }
        return list;
    }

    public async Task<int> AddAsync(ExpenditureDto expenseDto)
    {
        using var _context = await _contextFactory.CreateDbContextAsync();
        var expenseHead = await _context.ExpenditureHeads
                            .Where(a => a.ExpenditureHeadId == expenseDto.ExpenditureHeadId)
                            .AsNoTracking()
                            .FirstOrDefaultAsync();

        if (expenseHead is null)
        {
            throw new Exception("Invalid expense head");
        }

        var entity = new Expenditure
        {
            ExpenditureHeadId = expenseDto.ExpenditureHeadId,
            ParentHeadId = expenseHead.ParentHeadId,
            Amount = expenseDto.Amount,
            Remark = expenseDto.Remark,
            IsDeleted = false,
            ExpenditureDate = expenseDto.ExpenditureDate
        };

        _context.Expenditures.Add(entity);
        await _context.SaveChangesAsync();

        return entity.ExpenditureId;
    }

    public async Task<bool> RemoveAsync(int expenditureId)
    {
        using var _context = await _contextFactory.CreateDbContextAsync();
        var expenditure = await _context.Expenditures
                            .FirstOrDefaultAsync(a => a.ExpenditureId == expenditureId);

        if (expenditure is null)
        {
            throw new Exception("Invalid expenditure");
        }

        expenditure.IsDeleted = true;
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<int> UpdateAsync(ExpenditureDto expenseDto)
    {
        using var _context = await _contextFactory.CreateDbContextAsync();
        var expenditure = await _context.Expenditures
                            .FirstOrDefaultAsync(a => a.ExpenditureId == expenseDto.ExpenditureId);

        if (expenditure is null)
        {
            throw new Exception("Invalid expenditure");
        }

        if (expenditure.ExpenditureHeadId != expenseDto.ExpenditureHeadId)
        {
            var expenseHead = await _context.ExpenditureHeads
                            .Where(a => a.ExpenditureHeadId == expenseDto.ExpenditureHeadId)
                            .AsNoTracking()
                            .FirstOrDefaultAsync();

            if (expenseHead is null)
            {
                throw new Exception("Invalid expense head");
            }

            expenditure.ExpenditureHeadId = expenseDto.ExpenditureHeadId;
            expenditure.ParentHeadId = expenseHead.ParentHeadId;
        }

        expenditure.ExpenditureDate = expenseDto.ExpenditureDate;
        expenditure.Amount = expenseDto.Amount;
        expenditure.Remark = expenseDto.Remark;

        await _context.SaveChangesAsync();

        return expenseDto.ExpenditureId;
    }


    public async Task<decimal> GetTotalMonthExpenditureAsync(int year, int month)
    {
        using var _context = await _contextFactory.CreateDbContextAsync();
        DateTime start = new(year, month, 1, 0, 0, 0);
        DateTime end = start.AddMonths(1);

        return await _getTotalMonthExpenditureQuery(_context, start, end);
    }
}
