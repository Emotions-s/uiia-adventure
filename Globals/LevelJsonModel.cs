namespace uiia_adventure.Globals;
using System.Collections.Generic;
public class LevelJsonModel
{
    public string LevelName { get; set; }
    public string TilesetPath { get; set; }
    public string TilemapPath { get; set; }
    public string GroundPath { get; set; }
    public string WallPath { get; set; }
    public string LadderPath { get; set; }
    public string HazardPath { get; set; }
    public int[] SpawnPoint { get; set; }
    public int[] FinishPoint { get; set; }
    public List<string> Keys { get; set; }

    public List<LevelObjectData> Objects { get; set; } = new();
}