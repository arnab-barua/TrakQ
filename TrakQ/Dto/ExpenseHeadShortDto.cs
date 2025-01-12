namespace TrakQ.Dto;

public record ExpenseHeadShortDto
{
    public int Id { get; set; } = 0;
    public string HeadName { get; set; } = string.Empty;
    public decimal? FixedAmount { get; init; }
}
