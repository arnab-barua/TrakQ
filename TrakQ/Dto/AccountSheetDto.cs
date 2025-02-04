namespace TrakQ.Dto;

public record AccountSheetDto
{
    public int Id { get; set; }
    public int AccountId { get; set; }
    public string AccountName { get; set; } = string.Empty;
    public decimal OpeningBalance { get; set; } = 0;
    public decimal? ClosingBalance { get; set; }
    public int FiscalMonthId { get; init; }
    public Int16 Year { get; set; } = (short)DateTime.Now.Year;
    public Byte Month { get; set; } = (byte)DateTime.Now.Month;
}
