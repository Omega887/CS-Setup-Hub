namespace CESetupHub.Models;

public sealed class SetupItem
{
    public string Id { get; set; } = "";
    public string Name { get; set; } = "";
    public string Category { get; set; } = "";
    public string Description { get; set; } = "";
    public string Preset { get; set; } = "Custom";
    public bool Premium { get; set; }
    public bool RequiresRestart { get; set; }
    public string? WingetId { get; set; }
    public string? Command { get; set; }
    public string? VerifyCommand { get; set; }
    public string? Url { get; set; }
    public List<string> Tags { get; set; } = [];
}
