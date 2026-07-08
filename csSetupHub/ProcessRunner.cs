using System.Diagnostics;
using System.Text;

namespace CESetupHub.Services;

public sealed class ProcessRunner
{
    public async Task<(int ExitCode, string Output)> RunAsync(
        string fileName,
        string arguments,
        Action<string> onLine,
        CancellationToken cancellationToken)
    {
        var output = new StringBuilder();
        using var process = new Process();
        process.StartInfo = new ProcessStartInfo
        {
            FileName = fileName,
            Arguments = arguments,
            UseShellExecute = false,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            CreateNoWindow = true
        };

        process.OutputDataReceived += (_, e) => AppendLine(e.Data);
        process.ErrorDataReceived += (_, e) => AppendLine(e.Data);

        process.Start();
        process.BeginOutputReadLine();
        process.BeginErrorReadLine();
        await process.WaitForExitAsync(cancellationToken);
        return (process.ExitCode, output.ToString());

        void AppendLine(string? line)
        {
            if (string.IsNullOrWhiteSpace(line))
            {
                return;
            }

            output.AppendLine(line);
            onLine(line);
        }
    }
}
