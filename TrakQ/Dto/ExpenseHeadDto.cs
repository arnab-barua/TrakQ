namespace TrakQ.Dto;
public record ExpenseHeadDto
{
    public int Id { get; set; } = 0;
    public string HeadName { get; init; } = string.Empty;
    public int ParentHeadId { get; set; } = 0;
    public string? ParentName { get; init; }
    public string? Note { get; init; }
    public decimal? FixedAmount { get; init; }
    public decimal? Budget { get; init; }

    public List<ExpenseHeadDto> ChildHeads { get; init; } = [];

}
