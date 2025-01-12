using TrakQ.Db;
using TrakQ.Db.Data.Entities;
using TrakQ.Dto;
using Microsoft.EntityFrameworkCore;

namespace TrakQ.Service;
public sealed class ExpenseHeadService
{
    private readonly AppDbContext _context;

    public ExpenseHeadService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<ExpenseHeadDto>> GetAllAsync()
    {
        return await _context.ExpenditureHeads
            .Where(a => a.ParentHeadId == 0)
            .OrderBy(a => a.ExpenditureHeadId)
            .AsNoTracking()
            .Select(L1 => new ExpenseHeadDto
            {
                Id = L1.ExpenditureHeadId,
                HeadName = L1.HeadName,
                ParentHeadId = L1.ParentHeadId,
                Note = L1.Note,
                Budget = L1.Budget,
                FixedAmount = L1.FixedAmount,

                ChildHeads = _context.ExpenditureHeads
                                    .Where(b => b.ParentHeadId == L1.ExpenditureHeadId)
                                    .Select(L2 => new ExpenseHeadDto
                                    {
                                        Id = L2.ExpenditureHeadId,
                                        HeadName = L2.HeadName,
                                        ParentHeadId = L2.ParentHeadId,
                                        ParentName = L1.HeadName,
                                        Note = L2.Note,
                                        Budget = L2.Budget,
                                        FixedAmount = L2.FixedAmount,

                                        ChildHeads = _context.ExpenditureHeads
                                                        .Where(L3 => L3.ParentHeadId == L2.ExpenditureHeadId)
                                                        .Select(L3 => new ExpenseHeadDto
                                                        {
                                                            Id = L3.ExpenditureHeadId,
                                                            HeadName = L3.HeadName,
                                                            ParentHeadId = L3.ParentHeadId,
                                                            ParentName = L2.HeadName,
                                                            Note = L3.Note,
                                                            Budget = L3.Budget,
                                                            FixedAmount = L3.FixedAmount,
                                                            ChildHeads = _context.ExpenditureHeads
                                                                    .Where(L4 => L4.ParentHeadId == L3.ExpenditureHeadId)
                                                                    .Select(L4 => new ExpenseHeadDto
                                                                    {
                                                                        Id = L4.ExpenditureHeadId,
                                                                        HeadName = L4.HeadName,
                                                                        ParentHeadId = L4.ParentHeadId,
                                                                        ParentName = L3.HeadName,
                                                                        Note = L4.Note,
                                                                        Budget = L4.Budget,
                                                                        FixedAmount = L4.FixedAmount
                                                                    })
                                                                    .ToList()
                                                        })
                                                        .ToList(),
                                    })
                                    .ToList()
            })
            .ToListAsync();
    }


    public async Task<List<ExpenseHeadShortDto>> FilteredHeaders(string? term)
    {
        if (string.IsNullOrEmpty(term))
        {
            term = string.Empty;
        }

        term = term.ToLower();

        return await _context.ExpenditureHeads
            .Where(a => a.HeadName.ToLower().Contains(term))
            .OrderBy(a => a.HeadName)
            .AsNoTracking()
            .Select(a => new ExpenseHeadShortDto
            {
                Id = a.ExpenditureHeadId,
                HeadName = a.HeadName,
                FixedAmount = a.FixedAmount
            })
            .ToListAsync();
    }


    public async Task<List<ParentTypeExpenseHeadDto>> GetParentTypeHeadsAsync()
    {
        var data = await _context.ExpenditureHeads
                    .Where(a => a.ParentHeadId == 0)
                    .Select(a => new ParentTypeExpenseHeadDto
                    {
                        ExpenditureHeadId = a.ExpenditureHeadId,
                        HeadName = a.HeadName,
                        ChildHeads = _context.ExpenditureHeads
                                        .Where(cld => cld.ParentHeadId == a.ExpenditureHeadId)
                                        .Select(cld => KeyValuePair.Create(cld.ExpenditureHeadId, cld.HeadName))
                                        .ToList()
                    })
                    .ToListAsync();

        return data;
    }



    public async Task<int> AddExpenseHeadAsync(ExpenseHeadDto expenseHeadForm)
    {
        var entity = new ExpenditureHead
        {
            HeadName = expenseHeadForm.HeadName,
            ParentHeadId = expenseHeadForm.ParentHeadId,
            Note = expenseHeadForm.Note,
            FixedAmount = expenseHeadForm.FixedAmount,
            Budget = expenseHeadForm.Budget,
        };

        _context.ExpenditureHeads.Add(entity);
        await _context.SaveChangesAsync();

        return entity.ExpenditureHeadId;
    }

    public async Task<int> UpdateExpenseHeadAsync(ExpenseHeadDto expenseHeadForm)
    {
        var expenseHead = await _context.ExpenditureHeads
                            .FirstOrDefaultAsync(a => a.ExpenditureHeadId == expenseHeadForm.Id);

        if (expenseHead is null)
        {
            throw new Exception("Invalid expense head");
        }

        expenseHead.HeadName = expenseHeadForm.HeadName;
        expenseHead.ParentHeadId = expenseHeadForm.ParentHeadId;
        expenseHead.Note = expenseHeadForm.Note;
        expenseHead.FixedAmount = expenseHeadForm.FixedAmount;
        expenseHead.Budget = expenseHeadForm.Budget;

        await _context.SaveChangesAsync();
        return expenseHeadForm.Id;
    }
}
