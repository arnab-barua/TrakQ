using TrakQ.Db;
using TrakQ.Dto;

namespace TrakQ.Service;

public class ReportService
{

    private readonly ExpenditureService _expenditureService;
    private readonly FiscalMonthService _fiscalMonthService;
    private readonly IncomeService _incomeService;
    private readonly AccountSheetService _accountSheetService;
    private readonly AppDbContext _context;

    public ReportService(
        ExpenditureService expenditureService,
        IncomeService incomeService,
        AccountSheetService accountSheetService,
        FiscalMonthService fiscalMonthService,
        AppDbContext context)
    {
        _expenditureService = expenditureService;
        _incomeService = incomeService;
        _accountSheetService = accountSheetService;
        _fiscalMonthService = fiscalMonthService;
        _context = context;
    }

    public async Task<MonthSummeryDto> GetMonthSummeryaAsync(int year, int month)
    {
        MonthSummeryDto monthSummery = new()
        {
            TotalExpense = await _expenditureService.GetTotalMonthExpenditureAsync(year, month),
            TotalIncome = await _incomeService.GetTotalMonthIncomeAsync(year, month)
        };
        (monthSummery.TotalOpeningBalance, monthSummery.TotalClosingBalance) = await _accountSheetService.GetMonthlyTotalBalanceAsync(year, month);

        return monthSummery;
    }   
}
