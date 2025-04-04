namespace uiia_adventure.Globals;

using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

public static class TilesetCache
{
    private static Dictionary<string, Texture2D> _tilesets = new();

    public static Texture2D GetTileset(string tileset_path, ContentManager content)
    {
        if (!_tilesets.TryGetValue(tileset_path, out var texture))
        {
            Console.WriteLine($"Loading tileset: {tileset_path}");
            texture = content.Load<Texture2D>(tileset_path);
            _tilesets[tileset_path] = texture;
        }

        return texture;
    }
}