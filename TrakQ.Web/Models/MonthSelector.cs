namespace TrakQ.Web.Models;

public class MonthSelector
{
    public int Year { get; set; }
    public int Month { get; set; }

    public static readonly int[] AvailableYears = [2022, 2023, 2024, 2025, 2026, 2027];

    public static readonly (int Value, string Name)[] AvailableMonths =
    [
        (1, "January"), (2, "February"), (3, "March"), (4, "April"),
        (5, "May"), (6, "June"), (7, "July"), (8, "August"),
        (9, "September"), (10, "October"), (11, "November"), (12, "December")
    ];

    public static MonthSelector ForNow() =>
        new() { Year = DateTime.Now.Year, Month = DateTime.Now.Month };

    public string MonthName =>
        AvailableMonths.FirstOrDefault(m => m.Value == Month).Name ?? Month.ToString();
}
