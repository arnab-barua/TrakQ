using TrakQ.Web.Models;

namespace TrakQ.Web.Services;

public interface ISummaryReportService
{
    Task<MonthlySummaryData> GetSummaryAsync(int year, int month);
    Task<List<MonthlyTotals>> GetTrendAsync(int monthsBack);
}
