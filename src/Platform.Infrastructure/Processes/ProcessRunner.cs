using System.Diagnostics;

namespace Platform.Infrastructure.Processes
{
    public class ProcessRunner : IProcessRunner
    {
        public ProcessResult Run(string command, IEnumerable<string> arguments)
        {
            using Process process = new();
            process.StartInfo = new ProcessStartInfo(command, arguments)
            {
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            process.Start();
            string stdOut = process.StandardOutput.ReadToEnd();
            string stdErr = process.StandardError.ReadToEnd();
            process.WaitForExit();
            return new ProcessResult(process.ExitCode, stdOut, stdErr);
        }
    }
}
