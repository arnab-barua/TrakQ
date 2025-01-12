namespace TrakQ.Dto;

public class ExpenditureDto
{
    public int ExpenditureId { get; init; } = 0;
    public int ExpenditureHeadId { get; set; } = 0;
    public string ExpenditureHeadText { get; init; } = string.Empty;
    public DateTime ExpenditureDate { get; set; } = DateTime.Now;
    public decimal Amount { get; init; } = 0;
    public string? Remark { get; init; }
}


public sealed record ExpensesByDay
{
    public DateTime ExpenditureDate { get; set; } = DateTime.Now.Date;
    public decimal TotalAmount { get; init; } = 0;

    public List<ExpenditureDto> Expenses { get; set; } = [];
}