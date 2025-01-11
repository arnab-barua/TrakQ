namespace TrakQ.Dto;
public record ExpenseHeadDto
{
    public int Id { get; init; }
    public string HeadName { get; init; } = string.Empty;
    public int ParentHeadId { get; init; }
    public string? ParentName { get; init; }
    public string? Note { get; init; }
    public decimal? FixedAmount { get; init; }
    public decimal? Budget { get; init; }

    public IEnumerable<ExpenseHeadDto> ChildHeads { get; init; } = [];

}
