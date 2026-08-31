using TrakQ.Db;
using TrakQ.Db.Data.Entities;
using TrakQ.Dto;
using Microsoft.EntityFrameworkCore;

namespace TrakQ.Service;
public sealed class ExpenseHeadService
{
    private readonly IDbContextFactory<AppDbContext> _contextFactory;

    public ExpenseHeadService(IDbContextFactory<AppDbContext> contextFactory)
    {
        _contextFactory = contextFactory;
    }

    public async Task<List<ExpenseHeadDto>> GetAllAsync()
    {
        using var _context = await _contextFactory.CreateDbContextAsync();
        var allRows = await _context.ExpenditureHeads
            .OrderBy(a => a.ExpenditureHeadId)
            .AsNoTracking()
            .ToListAsync();


        return allRows
            .Where(a => a.ParentHeadId == 0)
            .OrderBy(a => a.ExpenditureHeadId)
            .Select(L1 => new ExpenseHeadDto
            {
                Id = L1.ExpenditureHeadId,
                Name = L1.HeadName,
                ParentHeadId = L1.ParentHeadId,
                Note = L1.Note,
                Budget = L1.Budget,
                FixedAmount = L1.FixedAmount,

                Children = allRows
                            .Where(b => b.ParentHeadId == L1.ExpenditureHeadId)
                            .Select(L2 => new ExpenseHeadDto
                            {
                                Id = L2.ExpenditureHeadId,
                                Name = L2.HeadName,
                                ParentHeadId = L2.ParentHeadId,
                                ParentName = L1.HeadName,
                                Note = L2.Note,
                                Budget = L2.Budget,
                                FixedAmount = L2.FixedAmount,

                                Children = allRows
                                            .Where(L3 => L3.ParentHeadId == L2.ExpenditureHeadId)
                                            .Select(L3 => new ExpenseHeadDto
                                            {
                                                Id = L3.ExpenditureHeadId,
                                                Name = L3.HeadName,
                                                ParentHeadId = L3.ParentHeadId,
                                                ParentName = L2.HeadName,
                                                Note = L3.Note,
                                                Budget = L3.Budget,
                                                FixedAmount = L3.FixedAmount,
                                                Children = allRows
                                                        .Where(L4 => L4.ParentHeadId == L3.ExpenditureHeadId)
                                                        .Select(L4 => new ExpenseHeadDto
                                                        {
                                                            Id = L4.ExpenditureHeadId,
                                                            Name = L4.HeadName,
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
            .ToList();
    }


    public async Task<List<ExpenseHeadDto>> GetAllOptimizedAsync()
    {
        using var _context = await _contextFactory.CreateDbContextAsync();
        var rows = await _context.ExpenditureHeads
            .AsNoTracking()
            .OrderBy(x => x.ExpenditureHeadId)
            .ToListAsync();

        // Convert to DTOs first
        var dtoList = rows.Select(r => new ExpenseHeadDto
        {
            Id = r.ExpenditureHeadId,
            Name = r.HeadName,
            ParentHeadId = r.ParentHeadId,
            Note = r.Note,
            Budget = r.Budget,
            FixedAmount = r.FixedAmount
        }).ToList();

        // Lookup by parent id
        var lookup = dtoList.ToLookup(x => x.ParentHeadId);

        // Assign children
        foreach (var item in dtoList)
        {
            var parent = dtoList.FirstOrDefault(p => p.Id == item.ParentHeadId);
            item.ParentName = parent?.Name;
            item.Children.AddRange(lookup[item.Id]);
        }

        // Return roots (ParentHeadId == 0)
        return lookup[0].ToList();
    }



    public async Task<List<ExpenseHeadShortDto>> FilteredHeaders(string? term)
    {
        using var _context = await _contextFactory.CreateDbContextAsync();
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
        using var _context = await _contextFactory.CreateDbContextAsync();
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
        using var _context = await _contextFactory.CreateDbContextAsync();
        var entity = new ExpenditureHead
        {
            HeadName = expenseHeadForm.Name,
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
        using var _context = await _contextFactory.CreateDbContextAsync();
        var expenseHead = await _context.ExpenditureHeads
                            .FirstOrDefaultAsync(a => a.ExpenditureHeadId == expenseHeadForm.Id);

        if (expenseHead is null)
        {
            throw new Exception("Invalid expense head");
        }

        expenseHead.HeadName = expenseHeadForm.Name;
        expenseHead.ParentHeadId = expenseHeadForm.ParentHeadId;
        expenseHead.Note = expenseHeadForm.Note;
        expenseHead.FixedAmount = expenseHeadForm.FixedAmount;
        expenseHead.Budget = expenseHeadForm.Budget;

        await _context.SaveChangesAsync();
        return expenseHeadForm.Id;
    }
}
