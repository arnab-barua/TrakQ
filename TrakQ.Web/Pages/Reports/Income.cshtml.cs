using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;
using TrakQ.Web.Models;
using TrakQ.Web.Services;

namespace TrakQ.Web.Pages.Reports;

public class IncomeModel : PageModel
{
    private readonly IIncomeReportService _incomeService;

    public IncomeModel(IIncomeReportService incomeService)
        => _incomeService = incomeService;

    public MonthSelector Selector { get; private set; } = MonthSelector.ForNow();
    public List<IncomeHeadGroup> Groups { get; private set; } = [];
    public decimal Total { get; private set; }
    public string IncomeChartJson { get; private set; } = "{}";

    public async Task OnGetAsync(int? year, int? month)
    {
        Selector = new MonthSelector
        {
            Year = year ?? DateTime.Now.Year,
            Month = month ?? DateTime.Now.Month
        };

        Groups = await _incomeService.GetMonthlyIncomeByHeadAsync(Selector.Year, Selector.Month);
        Total = Groups.Sum(g => g.SubTotal);

        IncomeChartJson = JsonSerializer.Serialize(new
        {
            labels = Groups.Select(g => g.HeadName).ToArray(),
            datasets = new[]
            {
                new
                {
                    label = "Income",
                    data = Groups.Select(g => g.SubTotal).ToArray(),
                    backgroundColor = "#198754",
                    borderRadius = 4
                }
            }
        });
    }
}
