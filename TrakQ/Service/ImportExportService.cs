using Microsoft.EntityFrameworkCore;
using TrakQ.Db;

namespace TrakQ.Service;
public sealed class ImportExportService
{
    private readonly AppDbContext _context;

    public ImportExportService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<bool> ImportBulkDataAsync(string rawQuery)
    {
        var formattedQuery = FormattableStringFactory.Create(rawQuery);

        // Execute batch query to insert data.
        int insertedRows = await _context.Database.ExecuteSqlAsync(formattedQuery);

        return true;
    }
}
