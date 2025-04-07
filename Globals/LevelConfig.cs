namespace uiia_adventure.Globals;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text.Json;
using Microsoft.Xna.Framework.Content;

public static class LevelConfig
{
    public static List<LevelData> Levels { get; private set; } = new();

    public static void Load(ContentManager content)
    {
        string path = Path.Combine(AppContext.BaseDirectory, "../../../Content/Data/level.json");
        Debug.WriteLine($"Loading level config from: {path}");

        if (!File.Exists(path))
        {
            Debug.WriteLine($"Error: File not found at {path}");
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
            foreach (var sub in map.Submap)
            {
                var level = new LevelData()
                {
                    LevelName = sub.LevelName,
                    MapPath = Path.Combine(map.BasePath, sub.TilemapPath),
                    DynamicMapPath = Path.Combine(map.BasePath, sub.DynamicTilemapPath),
                    LadderPath = Path.Combine(map.BasePath, sub.LadderPath),
                    GroundPath = Path.Combine(map.BasePath, sub.GroundPath),
                    WallPath = Path.Combine(map.BasePath, sub.WallPath),
                    HazardPath = Path.Combine(map.BasePath, sub.HazardPath),
                    TilesetPath = map.TilesetPath
                };
                Levels.Add(level);
            }
        }
        Debug.WriteLine($"Loaded {Levels.Count} levels.");
        foreach (var level in Levels)
        {
            Debug.WriteLine($"Level: {level.LevelName}, Map: {level.MapPath}, Ground: {level.GroundPath}, Hazard: {level.HazardPath}");
        }
    }

    public static LevelData? GetLevelByName(string name) =>
        Levels.Find(level => level.LevelName == name);

    private class RawLevelMap
    {
        public string TilesetPath { get; set; }
        public string BasePath { get; set; }
        public List<RawSubMap> Submap { get; set; } = new();
    }

    private class RawSubMap
    {
        public string LevelName { get; set; }
        public string TilemapPath { get; set; }
        public string DynamicTilemapPath { get; set; }
        public string LadderPath { get; set; }
        public string GroundPath { get; set; }
        public string WallPath { get; set; }
        public string HazardPath { get; set; }
    }
}
