using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace uiia_adventure.Components;
public class HazardTileComponent : IComponent
{
    public HashSet<Point> Tiles = new();
}

