using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;
using TrakQ.Web.Models;
using TrakQ.Web.Services;

namespace TrakQ.Web.Pages.Reports;

public class SummaryModel : PageModel
{
    private readonly ISummaryReportService _summaryService;

    public SummaryModel(ISummaryReportService summaryService)
        => _summaryService = summaryService;

    public MonthSelector Selector { get; private set; } = MonthSelector.ForNow();
    public MonthlySummaryData Summary { get; private set; } = new();
    public string IncomeExpenseChartJson { get; private set; } = "{}";
    public string TopHeadsChartJson { get; private set; } = "{}";

    public async Task OnGetAsync(int? year, int? month)
    {
        Selector = new MonthSelector
        {
            Year = year ?? DateTime.Now.Year,
            Month = month ?? DateTime.Now.Month
        };

        Summary = await _summaryService.GetSummaryAsync(Selector.Year, Selector.Month);

        IncomeExpenseChartJson = JsonSerializer.Serialize(new
        {
            labels = new[] { "Income", "Expense" },
            datasets = new[]
            {
                new
                {
                    data = new[] { Summary.TotalIncome, Summary.TotalExpense },
                    backgroundColor = new[] { "#198754", "#dc3545" },
                    hoverOffset = 4
                }
            }
        });

        TopHeadsChartJson = JsonSerializer.Serialize(new
        {
            labels = Summary.TopExpenseHeads.Select(h => h.HeadName).ToArray(),
            datasets = new[]
            {
                new
                {
                    label = "Amount",
                    data = Summary.TopExpenseHeads.Select(h => h.Amount).ToArray(),
                    backgroundColor = "#0d6efd"
                }
            }
        });
    }
}
