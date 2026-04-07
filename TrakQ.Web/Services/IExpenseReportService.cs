using TrakQ.Web.Models;

namespace TrakQ.Web.Services;

public interface IExpenseReportService
{
    Task<List<DailyExpenseRow>> GetMonthlyExpensesAsync(int year, int month);
    Task<List<ExpenseHeadTotalNode>> GetExpenseByHeadAsync(int year, int month);
}
