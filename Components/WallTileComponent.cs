using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace uiia_adventure.Components;

public class WallTileComponent : IComponent
{
    public HashSet<Point> Tiles = new();
}
