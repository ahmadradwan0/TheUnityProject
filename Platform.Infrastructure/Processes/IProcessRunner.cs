
namespace Platform.Infrastructure.Processes
{
    public interface IProcessRunner
    {
        ProcessResult Run(string command, IEnumerable<string> arguments);
    }
}
