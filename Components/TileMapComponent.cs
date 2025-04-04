using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace uiia_adventure.Components;
public class TileMapComponent : IComponent
{
    public Dictionary<Point, int> Grid = new();
    public int TileWidth;
    public int TileHeight;
    public Texture2D Tileset;
    public int TilesPerRowMap;
    public int TilesPerRowSet;
}