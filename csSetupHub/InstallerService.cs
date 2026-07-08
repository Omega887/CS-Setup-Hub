using CESetupHub.Models;

namespace CESetupHub.Services;

public sealed class InstallerService
{
    private readonly ProcessRunner _runner = new();
    private readonly string _logPath;

    public InstallerService()
    {
        var logDir = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),
            "CE Setup Hub");
        Directory.CreateDirectory(logDir);
        _logPath = Path.Combine(logDir, $"install-{DateTime.Now:yyyyMMdd-HHmmss}.log");
    }

    public string LogPath => _logPath;

    public async Task<InstallResult> InstallAsync(
        SetupItem item,
        Action<string> onLine,
        CancellationToken cancellationToken)
    {
        onLine($"== {item.Name} ==");
        await File.AppendAllTextAsync(_logPath, $"{DateTime.Now:u} START {item.Name}{Environment.NewLine}", cancellationToken);

        var command = BuildCommand(item);
        if (command is null)
        {
            var message = "No automatic installer is configured. Open the linked resource manually.";
            onLine(message);
            return new InstallResult(item.Name, false, message);
        }

        var result = await _runner.RunAsync(
            "powershell.exe",
            $"-NoProfile -ExecutionPolicy Bypass -Command \"{EscapePowerShell(command)}\"",
            asyncLine =>
            {
                onLine(asyncLine);
                File.AppendAllText(_logPath, asyncLine + Environment.NewLine);
            },
            cancellationToken);

        if (result.ExitCode != 0)
        {
            var message = $"Installer exited with code {result.ExitCode}.";
            onLine(message);
            return new InstallResult(item.Name, false, message);
        }

        if (!string.IsNullOrWhiteSpace(item.VerifyCommand))
        {
            onLine($"Verifying: {item.VerifyCommand}");
            var verify = await _runner.RunAsync(
                "powershell.exe",
                $"-NoProfile -Command \"{EscapePowerShell(item.VerifyCommand)}\"",
                onLine,
                cancellationToken);

            if (verify.ExitCode != 0)
            {
                return new InstallResult(item.Name, false, "Installed, but verification failed. PATH may need a new terminal or restart.");
            }
        }

        await File.AppendAllTextAsync(_logPath, $"{DateTime.Now:u} DONE {item.Name}{Environment.NewLine}", cancellationToken);
        return new InstallResult(item.Name, true, "Installed and verified.");
    }

    public static bool IsCommandAvailable(string command)
    {
        var path = Environment.GetEnvironmentVariable("PATH") ?? "";
        return path.Split(Path.PathSeparator)
            .Select(p => Path.Combine(p.Trim(), command))
            .Any(File.Exists);
    }

    private static string? BuildCommand(SetupItem item)
    {
        if (!string.IsNullOrWhiteSpace(item.Command))
        {
            return item.Command;
        }

        if (!string.IsNullOrWhiteSpace(item.WingetId))
        {
            return $"winget install --id {item.WingetId} --exact --accept-package-agreements --accept-source-agreements";
        }

        if (!string.IsNullOrWhiteSpace(item.Url))
        {
            return $"Start-Process '{item.Url}'";
        }

        return null;
    }

    private static string EscapePowerShell(string command)
    {
        return command.Replace("\"", "`\"");
    }
}
