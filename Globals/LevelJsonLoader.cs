using System.IO;
using System.Text.Json;
using uiia_adventure.Globals;

public static class LevelJsonLoader
{
    public static LevelJsonModel LoadFromFile(string filePath)
    {
        if (!File.Exists(filePath))
            throw new FileNotFoundException($"Level file not found: {filePath}");

        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            ReadCommentHandling = JsonCommentHandling.Skip,
            AllowTrailingCommas = true
        };

        string json = File.ReadAllText(filePath);
        return JsonSerializer.Deserialize<LevelJsonModel>(json, options);
    }
}