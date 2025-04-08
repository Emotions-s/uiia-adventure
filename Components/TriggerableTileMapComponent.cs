namespace uiia_adventure.Components;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

public class TriggerableTileMapComponent : IComponent
{
    public string TriggerId;
    public Dictionary<Point, int> Tiles = new();

}