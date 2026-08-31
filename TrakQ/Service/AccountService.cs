using Microsoft.EntityFrameworkCore;
using TrakQ.Db;

namespace TrakQ.Service;

public class AccountService
{
    private readonly IDbContextFactory<AppDbContext> _contextFactory;

    public AccountService(IDbContextFactory<AppDbContext> contextFactory)
    {
        _contextFactory = contextFactory;
    }

    public async Task<List<KeyValuePair<int, string>>> GetAllAccountsAsync()
    {
        using var _context = await _contextFactory.CreateDbContextAsync();
        return await _context.Accounts
            .AsNoTracking()
            .Select(x => new KeyValuePair<int, string>(x.AccountId, x.AccountName))
            .ToListAsync();
    }
}
