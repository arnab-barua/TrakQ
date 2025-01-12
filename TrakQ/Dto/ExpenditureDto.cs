namespace TrakQ.Dto;

public class ExpenditureDto
{
    public int ExpenditureId { get; init; } = 0;
    public int ExpenditureHeadId { get; init; } = 0;
    public string ExpenditureHeadText { get; init; } = string.Empty;
    public DateTime ExpenditureDate { get; init; } = DateTime.Now;
    public decimal Amount { get; init; } = 0;
    public string? Remark { get; init; }
}
