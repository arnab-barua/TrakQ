//using SQLite;
//using System.Linq.Expressions;

//namespace TrakQ.Db;
//public class TrakQDbContextWithPcl
//{
//    private SQLiteAsyncConnection _database;
//    public TrakQDbContextWithPcl()
//    {
//    }
//    async Task Init()
//    {
//        if (_database is not null)
//            return;

//        _database = new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);
//        var result = await _database.CreateTableAsync<ExpenditureHead>();
//    }

//    public async Task<List<T>> GetAllAsync<T>(Expression<Func<T, bool>> predicate) where T : new()
//    {
//        await Init();
//        var data = await _database.Table<T>()
//            .Where(predicate)
//            .ToListAsync();
//        return data;
//    }

//    public async Task<int> AddAsync<T>(T item)
//    {
//        await Init();
//        var result = await _database.InsertAsync(item);
//        return result;
//    }

//    public async Task<int> UpdateAsync<T>(T item)
//    {
//        await Init();
//        return await _database.UpdateAsync(item);
//    }

//    public async Task<T?> FirstOrDefaultAsync<T>(Expression<Func<T, bool>> predicate) where T : new()
//    {
//        await Init();
//        return await _database.Table<T>()
//            .FirstOrDefaultAsync(predicate);
//    }
//}
