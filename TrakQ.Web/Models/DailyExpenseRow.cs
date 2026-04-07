namespace TrakQ.Web.Models;

public record DailyExpenseRow(
    DateTime Date,
    string HeadName,
    decimal Amount,
    string? Remark
);
