
namespace Platform.Infrastructure.Logging
{
    public interface IAppLogger
    {
        public void ConsoleLog(string message, LogLevel level = LogLevel.Debug, bool headerDesign = false);
    }
}
