using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;
using TrakQ.Web.Models;
using TrakQ.Web.Services;

namespace TrakQ.Web.Pages.Reports;

public class ExpenseByHeadModel : PageModel
{
    private readonly IExpenseReportService _expenseService;

    public ExpenseByHeadModel(IExpenseReportService expenseService)
        => _expenseService = expenseService;

    public MonthSelector Selector { get; private set; } = MonthSelector.ForNow();
    public List<ExpenseHeadTotalNode> Roots { get; private set; } = [];
    public decimal GrandTotal { get; private set; }
    public string HeadShareChartJson { get; private set; } = "{}";

    public async Task OnGetAsync(int? year, int? month)
    {
        Selector = new MonthSelector
        {
            Year = year ?? DateTime.Now.Year,
            Month = month ?? DateTime.Now.Month
        };

        Roots = await _expenseService.GetExpenseByHeadAsync(Selector.Year, Selector.Month);
        GrandTotal = Roots.Sum(r => r.Total);

        HeadShareChartJson = JsonSerializer.Serialize(new
        {
            labels = Roots.Where(r => r.Total > 0).Select(r => r.HeadName).ToArray(),
            datasets = new[]
            {
                new
                {
                    data = Roots.Where(r => r.Total > 0).Select(r => r.Total).ToArray(),
                    backgroundColor = new[]
                    {
                        "#0d6efd", "#6610f2", "#6f42c1", "#d63384", "#dc3545",
                        "#fd7e14", "#ffc107", "#198754", "#20c997", "#0dcaf0"
                    },
                    hoverOffset = 4
                }
            }
        });
    }
}
