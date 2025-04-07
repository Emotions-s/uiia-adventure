using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace uiia_adventure.Components;
public class DynamicTileMapComponent : IComponent
{
    public Dictionary<Point, int> Grid = new();
}