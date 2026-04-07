namespace TrakQ.Web.Models;

public record MonthlyTotals(int Year, int Month, decimal Income, decimal Expense)
{
    public string Label => $"{new DateTime(Year, Month, 1):MMM yyyy}";
}
