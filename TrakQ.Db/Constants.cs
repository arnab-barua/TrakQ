﻿namespace TrakQ.Db;
public static class Constants
{
    public const string DatabaseFilename = "TrakQ.db3";

    //public const SQLite.SQLiteOpenFlags Flags =
    //    // open the database in read/write mode
    //    SQLite.SQLiteOpenFlags.ReadWrite |
    //    // create the database if it doesn't exist
    //    SQLite.SQLiteOpenFlags.Create |
    //    // enable multi-threaded database access
    //    SQLite.SQLiteOpenFlags.SharedCache;

    public static string ApplicationPath =>
        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "trakq");

    public static string DatabasePath =>
        Path.Combine(ApplicationPath, DatabaseFilename);
}
