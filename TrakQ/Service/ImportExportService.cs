using Microsoft.EntityFrameworkCore;
using TrakQ.Db;

namespace TrakQ.Service;
public sealed class ImportExportService
{
    private readonly IDbContextFactory<AppDbContext> _contextFactory;

    public ImportExportService(IDbContextFactory<AppDbContext> contextFactory)
    {
        _contextFactory = contextFactory;
    }

    public async Task<bool> ImportBulkDataAsync(string rawQuery)
    {
        using var _context = await _contextFactory.CreateDbContextAsync();
        var formattedQuery = FormattableStringFactory.Create(rawQuery);

        // Execute batch query to insert data.
        int insertedRows = await _context.Database.ExecuteSqlAsync(formattedQuery);

        return true;
    }


    public async Task<bool> ForceCheckpointForDbSync()
    {
        using var _context = await _contextFactory.CreateDbContextAsync();
        var formattedQuery = FormattableStringFactory.Create("PRAGMA wal_checkpoint(FULL);");

        // Execute batch query to insert data.
        int insertedRows = await _context.Database.ExecuteSqlAsync(formattedQuery);

        return true;
    }
}

