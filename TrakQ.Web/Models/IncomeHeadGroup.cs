namespace TrakQ.Web.Models;

public record IncomeRow(DateTime Date, decimal Amount, string? Remark);

public class IncomeHeadGroup
{
    public string HeadName { get; init; } = "";
    public List<IncomeRow> Rows { get; init; } = [];
    public decimal SubTotal => Rows.Sum(r => r.Amount);
}
