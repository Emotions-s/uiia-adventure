// File: Globals/LevelConfig.cs

using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace uiia_adventure.Globals;

public static class LevelConfig
{
    public static List<LevelData> Levels { get; private set; } = new();

    public static void Load(ContentManager content)
    {
        string path = Path.Combine(AppContext.BaseDirectory, "../../../Content/Data/level.json");
        Console.WriteLine($"Loading level config from: {path}");

        if (!File.Exists(path))
        {
            Console.WriteLine($"Error: File not found at {path}");
            return;
        }

        using var stream = File.OpenRead(path); // Use File.OpenRead instead of TitleContainer.OpenStream
        using var reader = new StreamReader(stream);
        string json = reader.ReadToEnd();

        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            ReadCommentHandling = JsonCommentHandling.Skip,
            AllowTrailingCommas = true
        };

        var maps = JsonSerializer.Deserialize<List<RawLevelMap>>(json, options)!;
        foreach (var map in maps)
        {
            foreach (var sub in map.submap)
            {
                var level = new LevelData(
                    sub.levelName,
                    map.basePath,
                    map.tilesetPath,
                    sub.tilemapPath,
                    sub.groundPath,
                    sub.hazardPath
                );
                Levels.Add(level);
            }
        }
        Console.WriteLine($"Loaded {Levels.Count} levels.");
        // print the result
        foreach (var level in Levels)
        {
            Console.WriteLine($"Level: {level.levelName}, Map: {level.mapPath}, Ground: {level.groundPath}, Hazard: {level.hazardPath}");
        }
    }

    public static LevelData? GetLevelByName(string name) =>
        Levels.Find(level => level.levelName == name);

    private class RawLevelMap
    {
        public string tilesetPath { get; set; }
        public string basePath { get; set; }
        public List<RawSubMap> submap { get; set; } = new();
    }

    private class RawSubMap
    {
        public string levelName { get; set; }
        public string tilemapPath { get; set; }
        public string groundPath { get; set; }
        public string hazardPath { get; set; }
    }
}
