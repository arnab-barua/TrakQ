using TrakQ.Dto;


namespace TrakQ.Service;
public class ExpenseHeadService
{
    //private readonly TrakQDbContext _dbContext;

    public ExpenseHeadService()
    {
        //_dbContext = dbContext;
    }

    public async Task<List<ExpenseHeadDto>> GetExpenseHeadsAsync()
    {
        //var expenseHeads = await _dbContext.GetAllAsync<ExpenditureHead>(a => a.ExpenditureHeadId > 0);
        //return expenseHeads.Select(L1 => new ExpenseHeadDto
        //{
        //    Id = L1.ExpenditureHeadId,
        //    HeadName = L1.HeadName,
        //    ParentHeadId = L1.ParentHeadId,
        //    Note = L1.Note,
        //    Budget = L1.Budget,
        //    FixedAmount = L1.FixedAmount,
        //    ChildHeads = []
        //}).ToList();
        return [];
    }


    public async Task<bool> CreateExpenseHeadAsync(ExpenseHeadDto newExpenseHead)
    {
        //var entity = new ExpenditureHead
        //{
        //    HeadName = newExpenseHead.HeadName,
        //    ParentHeadId = newExpenseHead.ParentHeadId,
        //    Note = newExpenseHead.Note,
        //    FixedAmount = newExpenseHead.FixedAmount,
        //    Budget = newExpenseHead.Budget,
        //};

        //await _dbContext.AddAsync<ExpenditureHead>(entity);

        return true;
    }

    public async Task<bool> UpdateExpenseHeadAsync(ExpenseHeadDto updatedExpenseHead)
    {
        //var expenseHead = await _dbContext
        //                    .FirstOrDefaultAsync<ExpenditureHead>(a => a.ExpenditureHeadId == updatedExpenseHead.Id);

        //if (expenseHead is null)
        //{
        //    throw new Exception("Invalid expense head");
        //}

        //expenseHead.HeadName = updatedExpenseHead.HeadName;
        //expenseHead.ParentHeadId = updatedExpenseHead.ParentHeadId;
        //expenseHead.Note = updatedExpenseHead.Note;
        //expenseHead.FixedAmount = updatedExpenseHead.FixedAmount;
        //expenseHead.Budget = updatedExpenseHead.Budget;

        //await _dbContext.UpdateAsync<ExpenditureHead>(expenseHead);
        return true;
    }
}
