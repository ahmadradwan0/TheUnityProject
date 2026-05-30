
namespace Platform.Infrastructure.Processes
{
    public record ProcessResult(int ExitCode, string StdOut, string StdErr);
}
