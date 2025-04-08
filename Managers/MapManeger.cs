namespace uiia_adventure.Managers;

using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using uiia_adventure.Components;
using uiia_adventure.Core;
using uiia_adventure.Globals;

public class MapManager
{
    public GameObject LoadLevel(LevelJsonModel levelData, ContentManager content)
    {
        GameObject mapObj = new GameObject();
        mapObj.Name = GameConstants.TileMapName;

        string basePath;
#if DEBUG
        basePath = Path.Combine(AppContext.BaseDirectory, "../../../Content");
#else
    basePath = Path.Combine(AppContext.BaseDirectory, "Content");
#endif

        // load hazard
        HazardTileComponent hazardTileComponent = new();
        hazardTileComponent.Tiles = loadTileCsvToPos(Path.Combine(basePath, levelData.HazardPath));
        // show all points of hazard
        mapObj.AddComponent(hazardTileComponent);

        // load ground
        GroundTileComponent groundTileComponent = new();
        groundTileComponent.Tiles = loadTileCsvToPos(Path.Combine(basePath, levelData.GroundPath));
        mapObj.AddComponent(groundTileComponent);

        // load wall
        WallTileComponent wallTileComponent = new();
        wallTileComponent.Tiles = loadTileCsvToPos(Path.Combine(basePath, levelData.WallPath));
        mapObj.AddComponent(wallTileComponent);

        // load ladder
        LadderComponent ladderComponent = new();
        ladderComponent.Tiles = loadTileCsvToPos(Path.Combine(basePath, levelData.LadderPath));
        mapObj.AddComponent(ladderComponent);

        // load map
        TileMapComponent tileMapComponent = LoadTileMapCsvToComponent(Path.Combine(basePath, levelData.TilemapPath));
        Texture2D tileset = ResourceCache.GetTexture2D(levelData.TilesetPath, content);
        tileMapComponent.Tileset = tileset;
        tileMapComponent.TilesPerRowSet = tileset.Width / tileMapComponent.TileWidth;
        mapObj.AddComponent(tileMapComponent);

        return mapObj;
    }

    private TileMapComponent LoadTileMapCsvToComponent(string filePath)
    {
        if (!File.Exists(filePath))
            throw new FileNotFoundException("Map file not found", filePath);
        string[] lines = File.ReadAllLines(filePath);
        int rows = lines.Length;
        int cols = lines[0].Split(',').Length;
        var grid = new Dictionary<Point, int>();

        for (int y = 0; y < rows; y++)
        {
            var line = lines[y].Split(',');
            for (int x = 0; x < cols; x++)
            {
                int v = int.Parse(line[x]);
                if (v >= 0)
                {
                    grid[new Point(x, y)] = v;
                }
            }
        }

        ;

        TileMapComponent tileMapComponent = new()
        {
            Tiles = grid,
            TileWidth = GameConstants.TileSize,
            TileHeight = GameConstants.TileSize,
            Tileset = null,
            TilesPerRowMap = cols,
        };

        return tileMapComponent;
    }

    private HashSet<Point> loadTileCsvToPos(string filePath)
    {
        if (!File.Exists(filePath))
            throw new FileNotFoundException("Map file not found", filePath);
        HashSet<Point> points = new();
        string[] lines = File.ReadAllLines(filePath);
        int rows = lines.Length;
        int cols = lines[0].Split(',').Length;
        var grid = new Dictionary<Point, int>();

        for (int y = 0; y < rows; y++)
        {
            var line = lines[y].Split(',');
            for (int x = 0; x < cols; x++)
            {
                int v = int.Parse(line[x]);
                if (v >= 0)
                {
                    points.Add(new Point(x, y));
                }
            }
        }
        return points;
    }
}