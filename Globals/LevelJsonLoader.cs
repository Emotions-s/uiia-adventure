using System;
using System.IO;
using System.Text.Json;
using uiia_adventure.Globals;

public static class LevelJsonLoader
{
    public static LevelJsonModel LoadFromFile(string filePath)
    {


        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            ReadCommentHandling = JsonCommentHandling.Skip,
            AllowTrailingCommas = true
        };

        string basePath;

#if DEBUG
        basePath = Path.Combine(AppContext.BaseDirectory, "../../../Content");
#else
    basePath = Path.Combine(AppContext.BaseDirectory, "Content");
#endif

        string jsonPath = Path.Combine(basePath, filePath);

        if (!File.Exists(jsonPath))
            throw new FileNotFoundException($"Level file not found: {jsonPath}");

        string json = File.ReadAllText(jsonPath);
        return JsonSerializer.Deserialize<LevelJsonModel>(json, options);
    }
}