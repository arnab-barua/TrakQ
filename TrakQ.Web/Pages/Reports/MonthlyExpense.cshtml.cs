using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;
using TrakQ.Web.Models;
using TrakQ.Web.Services;

namespace TrakQ.Web.Pages.Reports;

public class MonthlyExpenseModel : PageModel
{
    private readonly IExpenseReportService _expenseService;

    public MonthlyExpenseModel(IExpenseReportService expenseService)
        => _expenseService = expenseService;

    public MonthSelector Selector { get; private set; } = MonthSelector.ForNow();
    public List<DailyExpenseRow> Expenses { get; private set; } = [];
    public decimal Total { get; private set; }
    public string DailyChartJson { get; private set; } = "{}";

    public async Task OnGetAsync(int? year, int? month)
    {
        Selector = new MonthSelector
        {
            Year = year ?? DateTime.Now.Year,
            Month = month ?? DateTime.Now.Month
        };

        Expenses = await _expenseService.GetMonthlyExpensesAsync(Selector.Year, Selector.Month);
        Total = Expenses.Sum(e => e.Amount);

        // Group by day for chart
        var byDay = Expenses
            .GroupBy(e => e.Date.Day)
            .OrderBy(g => g.Key)
            .ToList();

        DailyChartJson = JsonSerializer.Serialize(new
        {
            labels = byDay.Select(g => $"Day {g.Key}").ToArray(),
            datasets = new[]
            {
                new
                {
                    label = "Expense",
                    data = byDay.Select(g => g.Sum(e => e.Amount)).ToArray(),
                    backgroundColor = "#dc3545",
                    borderRadius = 4
                }
            }
        });
    }
}
