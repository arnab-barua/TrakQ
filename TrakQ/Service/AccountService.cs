using Microsoft.EntityFrameworkCore;
using TrakQ.Db;

namespace TrakQ.Service;

public class AccountService
{
    private readonly AppDbContext _context;

    public AccountService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<KeyValuePair<int, string>>> GetAllAccountsAsync()
    {
        return await _context.Accounts
            .AsNoTracking()
            .Select(x => new KeyValuePair<int, string>(x.AccountId, x.AccountName))
            .ToListAsync();
    }
}
