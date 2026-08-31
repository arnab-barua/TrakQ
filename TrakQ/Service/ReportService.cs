using Microsoft.EntityFrameworkCore;
using TrakQ.Db;
using TrakQ.Dto;

namespace TrakQ.Service;

public class ReportService
{

    private readonly ExpenditureService _expenditureService;
    private readonly FiscalMonthService _fiscalMonthService;
    private readonly IncomeService _incomeService;
    private readonly AccountSheetService _accountSheetService;
    private readonly IDbContextFactory<AppDbContext> _contextFactory;

    public ReportService(
        ExpenditureService expenditureService,
        IncomeService incomeService,
        AccountSheetService accountSheetService,
        FiscalMonthService fiscalMonthService,
        IDbContextFactory<AppDbContext> contextFactory)
    {
        _expenditureService = expenditureService;
        _incomeService = incomeService;
        _accountSheetService = accountSheetService;
        _fiscalMonthService = fiscalMonthService;
        _contextFactory = contextFactory;
    }

    public async Task<MonthSummeryDto> GetMonthSummeryaAsync(int year, int month)
    {
        using var _context = await _contextFactory.CreateDbContextAsync();
        MonthSummeryDto monthSummery = new()
        {
            TotalExpense = await _expenditureService.GetTotalMonthExpenditureAsync(year, month),
            TotalIncome = await _incomeService.GetTotalMonthIncomeAsync(year, month)
        };
        (monthSummery.TotalOpeningBalance, monthSummery.TotalClosingBalance) = await _accountSheetService.GetMonthlyTotalBalanceAsync(year, month);

        return monthSummery;
    }   
}

