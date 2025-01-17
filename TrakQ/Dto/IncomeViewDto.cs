namespace TrakQ.Dto;

public sealed record IncomeViewDto
{
    public int IncomeId { get; set; }
    public int IncomeHeadId { get; set; }
    public string IncomeHeadName { get; set; } = string.Empty;
    public DateTime IncomeDate { get; set; } = DateTime.Now;

    public decimal Amount { get; set; } = 0;
    public string? Remark { get; set; }
}
