using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace uiia_adventure.Components;

public class LadderComponent : IComponent
{
    public HashSet<Point> Tiles = new();
}