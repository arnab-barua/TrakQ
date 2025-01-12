namespace TrakQ.Dto;

public record ExpenseHeadShortDto
{
    public int Id { get; init; }
    public string HeadName { get; init; } = string.Empty;
    public decimal? FixedAmount { get; init; }
}
