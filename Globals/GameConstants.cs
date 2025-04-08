using Microsoft.Xna.Framework.Graphics;

namespace uiia_adventure.Globals;

public static class GameConstants
{
    public const int TileSize = 64;

    public static Texture2D Pixel { get; set; } = null!;

    public static string MeowBowName = "MeowBow";
    public static string MeowSwordName = "MeowSword";

    public static string TileMapName = "TileMap";
}