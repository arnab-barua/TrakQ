using Microsoft.EntityFrameworkCore;
using TrakQ.Db;
using TrakQ.Db.Data.Entities;
using TrakQ.Dto;

namespace TrakQ.Service;

public sealed class ExpenditureService
{
    private readonly AppDbContext _context;

    public ExpenditureService(AppDbContext context)
    {
        _context = context;
    }



    public async Task<List<ExpenditureDto>> GetMonthDataAsync(int year, int month)
    {
        DateTime start = new(year, month, 1, 0, 0, 0);
        DateTime end = start.AddMonths(1);

        return await _context.Expenditures
                            .Where(a => a.ExpenditureDate >= start
                                    && a.ExpenditureDate < end
                                    && !a.IsDeleted)
                            .Include(a => a.ExpenditureHead)
                            .OrderBy(a => a.ExpenditureDate.Date)
                            .AsNoTracking()
                            .Select(a => new ExpenditureDto
                            {
                                ExpenditureId = a.ExpenditureId,
                                ExpenditureHeadId = a.ExpenditureHeadId,
                                ExpenditureHeadText = a.ExpenditureHead.HeadName,
                                ExpenditureDate = a.ExpenditureDate,
                                Amount = a.Amount,
                                Remark = a.Remark
                            })
                            .ToListAsync();
    }

    public async Task<int> AddAsync(ExpenditureDto expenseDto)
    {
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
        DateTime start = new(year, month, 1, 0, 0, 0);
        DateTime end = start.AddMonths(1);

        return await _context.Expenditures
                            .Where(a => a.ExpenditureDate >= start
                                    && a.ExpenditureDate < end
                                    && !a.IsDeleted)
                            .SumAsync(a => a.Amount);
    }
}
