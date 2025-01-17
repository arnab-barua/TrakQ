namespace TrakQ.Dto;

public record MonthSummeryDto
{
    public decimal TotalIncome { get; set; } = 0;
    public decimal TotalExpense { get; set; } = 0;
    public decimal TotalOpeningBalance { get; set; } = 0;
    public decimal TotalClosingBalance { get; set; } = 0;


    public decimal TotalSourceOfFund { get; set; } = 0;
    public decimal RemainingBalance { get; set; } = 0;
    public decimal Difference { get; set; } = 0;

}
