namespace uiia_adventure.Components;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

public class DebugComponent : IComponent
{
    public HashSet<Rectangle> PlayerTiles = new();

}
