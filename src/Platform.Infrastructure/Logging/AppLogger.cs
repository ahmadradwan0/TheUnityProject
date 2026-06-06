namespace Platform.Infrastructure.Logging;

public class AppLogger : IAppLogger
{
    public void ConsoleLog(string message, LogLevel level = LogLevel.Information , bool headerDesign = false)
    {
        string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        ConsoleColor color = GetLevelColor(level);
        string label = GetLevelLabel(level);

        Console.ForegroundColor = color;
        Console.WriteLine($"[{timestamp}] [{label}] {message}");
        Console.ResetColor();
        if (headerDesign)
        {
            Console.WriteLine($"===================================");
        }
    }

    private ConsoleColor GetLevelColor(LogLevel level)
    {
        return level switch
        {
            LogLevel.Debug => ConsoleColor.Gray,
            LogLevel.Information => ConsoleColor.Green,
            LogLevel.Warning => ConsoleColor.Yellow,
            LogLevel.Error => ConsoleColor.Red,
            _ => ConsoleColor.White
        };
    }

    private string GetLevelLabel(LogLevel level)
    {
        return level switch
        {
            LogLevel.Debug => "DBG",
            LogLevel.Information => "INF",
            LogLevel.Warning => "WRN",
            LogLevel.Error => "ERR",
            _ => "???"
        };
    }
}