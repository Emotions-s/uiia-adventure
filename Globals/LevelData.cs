namespace uiia_adventure.Globals;

public class LevelData
{
    public string LevelName { get; set; }
    public string TilesetPath { get; set; }
    public string MapPath { get; set; }
    public string GroundPath { get; set; }
    public string WallPath { get; set; }
    public string HazardPath { get; set; }

    public LevelData() { }

    public LevelData(string levelName, string basePath, string tilesetPath, string mapPath, string groundPath, string wallPath, string hazardPath)
    {
        this.LevelName = levelName;
        this.TilesetPath = tilesetPath;
        this.MapPath = basePath + mapPath;
        this.GroundPath = basePath + groundPath;
        this.WallPath = basePath + wallPath;
        this.HazardPath = basePath + hazardPath;
    }
}
