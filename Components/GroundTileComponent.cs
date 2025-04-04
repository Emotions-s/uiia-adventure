using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace uiia_adventure.Components;

public class GroundTileComponent : IComponent
{
    public HashSet<Point> Tiles = new();
}
