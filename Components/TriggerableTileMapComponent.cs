using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace uiia_adventure.Components;
public class TriggerableTileMapComponent : IComponent
{
    public string TriggerId;
    public Dictionary<Point, int> Grid = new();

}