using System;
using System.IO;
using System.Threading.Tasks;

namespace TrakQ.Service;

public class ExceptionLoggerService
{
    private static readonly string LogFileName = "trakq_error_log.txt";
    private static string StaticLogFilePath => Path.Combine(FileSystem.AppDataDirectory, LogFileName);
    private readonly string _logFilePath;

    public ExceptionLoggerService()
    {
        _logFilePath = StaticLogFilePath;
    }

    public static void Log(Exception ex)
    {
        try
        {
            var logMessage = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] ERROR: {ex.Message}{Environment.NewLine}" +
                             $"STACK TRACE: {ex.StackTrace}{Environment.NewLine}" +
                             $"INNER EXCEPTION: {ex.InnerException?.Message}{Environment.NewLine}" +
                             new string('-', 80) + Environment.NewLine;

            File.AppendAllText(StaticLogFilePath, logMessage);
        }
        catch
        {
            // Fail silently to prevent recursive crash
        }
    }

    public static void Log(string message)
    {
        try
        {
            var logMessage = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] INFO: {message}{Environment.NewLine}" +
                             new string('-', 80) + Environment.NewLine;

            File.AppendAllText(StaticLogFilePath, logMessage);
        }
        catch
        {
            // Fail silently
        }
    }

    public void LogException(Exception ex)
    {
        Log(ex);
    }

    public async Task<string> GetLogsAsync()
    {
        if (File.Exists(_logFilePath))
        {
            return await File.ReadAllTextAsync(_logFilePath);
        }
        return "No logs found.";
    }

    public void ClearLogs()
    {
        if (File.Exists(_logFilePath))
        {
            File.Delete(_logFilePath);
        }
    }

    public string GetLogFilePath() => _logFilePath;
}

