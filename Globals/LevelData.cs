using System.Diagnostics.Contracts;
using System.Numerics;

namespace uiia_adventure.Globals;

public class LevelData
{
    public string LevelName { get; set; }
    public string TilesetPath { get; set; }
    public string MapPath { get; set; }
    public string DynamicMapPath { get; set; }
    public string LadderPath { get; set; }
    public string GroundPath { get; set; }
    public string WallPath { get; set; }
    public string HazardPath { get; set; }

    public Vector2 SpawnPoint { get; set; } = new Vector2(100, 200);

    public LevelData() { }
}
