namespace TrakQ.Web.Models;

public record ExpenseHeadShare(string HeadName, decimal Amount);

public class MonthlySummaryData
{
    public int Year { get; init; }
    public int Month { get; init; }
    public decimal TotalIncome { get; init; }
    public decimal TotalExpense { get; init; }
    public decimal TotalOpeningBalance { get; init; }
    public decimal TotalClosingBalance { get; init; }
    public decimal NetSavings => TotalIncome - TotalExpense;
    public List<ExpenseHeadShare> TopExpenseHeads { get; init; } = [];
}
