namespace uiia_adventure.Globals;

public class LevelData
{
    public string levelName { get; set; }
    public string tilesetPath { get; set; }
    public string mapPath { get; set; }
    public string groundPath { get; set; }
    public string hazardPath { get; set; }

    public LevelData() { }

    public LevelData(string levelName, string basePath, string tilesetPath, string mapPath, string groundPath, string hazardPath)
    {
        this.levelName = levelName;
        this.tilesetPath = tilesetPath;
        this.mapPath = basePath + mapPath;
        this.groundPath = basePath + groundPath;
        this.hazardPath = basePath + hazardPath;
    }
}
