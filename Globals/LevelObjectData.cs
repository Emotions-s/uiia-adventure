namespace uiia_adventure.Globals;

using System;
using System.Collections.Generic;
public class LevelObjectData
{
    public string Type { get; set; }
    public int[] Position { get; set; }
    public string Texture { get; set; }
    public int[] Size { get; set; }

    public List<string> Targets { get; set; }
    public string TriggerId { get; set; }
    public int Health { get; set; }
    public Dictionary<string, int> Tiles { get; set; }

    public int[] Direction { get; set; }

    public float Cooldown { get; set; }
}