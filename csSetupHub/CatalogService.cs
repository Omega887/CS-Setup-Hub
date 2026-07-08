using System.Text.Json;
using CESetupHub.Models;

namespace CESetupHub.Services;

public sealed class CatalogService
{
    private readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        WriteIndented = true
    };

    public IReadOnlyList<SetupItem> Load()
    {
        var path = Path.Combine(AppContext.BaseDirectory, "Resources", "catalog.json");
        if (!File.Exists(path))
        {
            path = Path.Combine(Environment.CurrentDirectory, "Resources", "catalog.json");
        }

        var json = File.ReadAllText(path);
        return JsonSerializer.Deserialize<List<SetupItem>>(json, _jsonOptions) ?? [];
    }
}
