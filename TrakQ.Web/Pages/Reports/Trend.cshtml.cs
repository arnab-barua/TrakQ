using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;
using TrakQ.Web.Models;
using TrakQ.Web.Services;

namespace TrakQ.Web.Pages.Reports;

public class TrendModel : PageModel
{
    private readonly ISummaryReportService _summaryService;

    public TrendModel(ISummaryReportService summaryService)
        => _summaryService = summaryService;

    public List<MonthlyTotals> Trend { get; private set; } = [];
    public string TrendChartJson { get; private set; } = "{}";
    public int MonthsBack { get; private set; } = 12;

    public async Task OnGetAsync(int? months)
    {
        MonthsBack = months ?? 12;
        if (MonthsBack < 2) MonthsBack = 2;
        if (MonthsBack > 36) MonthsBack = 36;

        Trend = await _summaryService.GetTrendAsync(MonthsBack);

        TrendChartJson = JsonSerializer.Serialize(new
        {
            labels = Trend.Select(t => t.Label).ToArray(),
            datasets = new object[]
            {
                new
                {
                    label = "Income",
                    data = Trend.Select(t => t.Income).ToArray(),
                    borderColor = "#198754",
                    backgroundColor = "rgba(25,135,84,0.1)",
                    fill = true,
                    tension = 0.3
                },
                new
                {
                    label = "Expense",
                    data = Trend.Select(t => t.Expense).ToArray(),
                    borderColor = "#dc3545",
                    backgroundColor = "rgba(220,53,69,0.1)",
                    fill = true,
                    tension = 0.3
                }
            }
        });
    }
}
