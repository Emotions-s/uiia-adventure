namespace uiia_adventure.Managers;

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

    public GameObject LoadLevel(LevelData levelData, ContentManager content)
    {
        GameObject mapObj = new GameObject();
        mapObj.Name = "TileMap";

        // load hazard
        HazardTileComponent hazardTileComponent = new();
        hazardTileComponent.Tiles = loadTileCsvToPos(levelData.HazardPath);
        mapObj.AddComponent(hazardTileComponent);

        // load ground
        GroundTileComponent groundTileComponent = new();
        groundTileComponent.Tiles = loadTileCsvToPos(levelData.GroundPath);
        mapObj.AddComponent(groundTileComponent);

        WallTileComponent wallTileComponent = new();
        wallTileComponent.Tiles = loadTileCsvToPos(levelData.WallPath);
        mapObj.AddComponent(wallTileComponent);

        // load map
        TileMapComponent tileMapComponent = LoadTileMapCsvToComponent(levelData.MapPath);
        Texture2D tileset = TilesetCache.GetTileset(levelData.TilesetPath, content);
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
                if (v > 0)
                {
                    grid[new Point(x, y)] = v;
                }
            }
        }

        ;

        TileMapComponent tileMapComponent = new()
        {
            Grid = grid,
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
                if (v > 0)
                {
                    points.Add(new Point(x, y));
                }
            }
        }
        return points;
    }
}