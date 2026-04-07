using TrakQ.Web.Models;

namespace TrakQ.Web.Services;

public interface IIncomeReportService
{
    Task<List<IncomeHeadGroup>> GetMonthlyIncomeByHeadAsync(int year, int month);
    Task<List<MonthlyTotals>> GetMonthlyIncomeTotalsAsync(int monthsBack);
}
