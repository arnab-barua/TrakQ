using Microsoft.EntityFrameworkCore;
using TrakQ.Db;
using TrakQ.Db.Data.Entities;
using TrakQ.Dto;


namespace TrakQ.Service;

public class AccountSheetService
{
    private readonly AppDbContext _context;
    private readonly FiscalMonthService _fiscalMonthService;

    public AccountSheetService(AppDbContext context, FiscalMonthService fiscalMonthService)
    {
        _context = context;
        _fiscalMonthService = fiscalMonthService;
    }



    public async Task<List<AccountSheetDto>> GetAccountBalancesAsync(int year, int month)
    {
        var fiscalMonth = await _fiscalMonthService.GetFiscalMonth(year, month);

        if (fiscalMonth is null)
        {
            return [];
        }

        return await _context.AccountSheets
                        .Where(a => a.FiscalMonthId == fiscalMonth.Id)
                        .Include(a => a.Account)
                        .Include(a => a.FiscalMonth)
                        .AsNoTracking()
                        .Select(a => new AccountSheetDto
                        {
                            Id = a.Id,
                            AccountId = a.AccountId,
                            AccountName = a.Account.AccountName,
                            OpeningBalance = a.OpeningBalance,
                            ClosingBalance = a.ClosingBalance,
                            FiscalMonthId = a.FiscalMonthId,
                            Year = a.FiscalMonth.Year,
                            Month = a.FiscalMonth.Month
                        })
                        .ToListAsync();
    }    

    public async Task<int> AddAsync(AccountSheetDto formDto)
    {
        var account = await _context.Accounts
                        .Where(a => a.AccountId == formDto.AccountId)
                        .AsNoTracking()
                        .FirstOrDefaultAsync();

        if (account is null)
        {
            throw new Exception("Invalid account");
        }

        // Check if balance for this account for this month already added.
        bool isAlreadyAdded = await IsAccountSheetAlreadyAddedInThisFiscaLMonth(formDto.Year, formDto.Month, formDto.AccountId);

        if (isAlreadyAdded)
        {
            throw new Exception("Account balance already added for this fiscal month");
        }

        var accountSheet = new AccountSheet
        {
            AccountId = formDto.AccountId,
            OpeningBalance = formDto.OpeningBalance,
            ClosingBalance = formDto.ClosingBalance
        };

        var fiscalMonth = await _fiscalMonthService.GetFiscalMonth(formDto.Year, formDto.Month);

        if (fiscalMonth is null)
        {
            accountSheet.FiscalMonth = new FiscalMonth
            {
                Year = formDto.Year,
                Month = formDto.Month
            };
        }
        else
        {
            accountSheet.FiscalMonthId = fiscalMonth.Id;
        }

        _context.AccountSheets.Add(accountSheet);
        await _context.SaveChangesAsync();

        return accountSheet.Id;
    }

    public async Task<int> UpdateAsync(AccountSheetDto formDto)
    {
        AccountSheet? accountSheet = await _context.AccountSheets
                            .FirstOrDefaultAsync(a => a.Id == formDto.Id);

        if (accountSheet is null)
        {
            throw new Exception("Invalid account sheet!");
        }
        

        // Check If account is changed.
        if (accountSheet.AccountId != formDto.AccountId)
        {
            throw new Exception("Account can't be changed!");
        }

        var fiscalMonth = await _fiscalMonthService.GetFiscalMonth(formDto.Year, formDto.Month);

        if (fiscalMonth is null)
        {
            accountSheet.FiscalMonth = new FiscalMonth
            {
                Year = formDto.Year,
                Month = formDto.Month
            };
        }
        else
        {
            accountSheet.FiscalMonthId = fiscalMonth.Id;
        }

        accountSheet.OpeningBalance = formDto.OpeningBalance;
        accountSheet.ClosingBalance = formDto.ClosingBalance;

        await _context.SaveChangesAsync();

        return formDto.Id;
    }



    public async Task<Tuple<decimal, decimal>> GetMonthlyTotalBalanceAsync(int year, int month)
    {
        var fiscalMonth = await _fiscalMonthService.GetFiscalMonth(year, month);

        if (fiscalMonth is null)
        {
            return new Tuple<decimal, decimal>(0, 0);
        }

        var data = await _context.AccountSheets
                        .Where(a => a.FiscalMonthId == fiscalMonth.Id)
                        .Select(a => new
                        {
                            a.OpeningBalance,
                            a.ClosingBalance
                        })
                        .ToListAsync();

        return new Tuple<decimal, decimal>(
            data.Sum(a => a.OpeningBalance),
            data.Sum(a => a.ClosingBalance ?? 0)
        );
    }


    private async Task<bool> IsAccountSheetAlreadyAddedInThisFiscaLMonth(int year, int month, int accountId)
    {
        return await _context.AccountSheets
                        .AnyAsync(a => a.AccountId == accountId
                                    && a.FiscalMonth.Year == year
                                    && a.FiscalMonth.Month == month);
    }
}
