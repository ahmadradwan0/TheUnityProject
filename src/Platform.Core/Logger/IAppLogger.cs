
namespace Platform.Core.Logger
{
    public interface IAppLogger
    {
        public void ConsoleLog(string message, LogLevel level, bool headerDesign = false);
    }
}
