using Microsoft.Xna.Framework;
using System.Collections.Generic;
using uiia_adventure.Core;
using uiia_adventure.Globals;

namespace uiia_adventure.Systems;

public class CameraSystem : SystemBase
{
    private int _mapWidthInTiles;
    private readonly int _screenWidth;
    private readonly int _tileSize;

    public Matrix Transform { get; private set; }
    public float CameraX { get; private set; }

    public CameraSystem(int screenWidth, int screenHeight)
    {
        _screenWidth = screenWidth;
        _tileSize = GameConstants.TileSize;
        _mapWidthInTiles = 0; // will set later
    }

    public void SetMapWidthInTiles(int mapWidthInTiles)
    {
        _mapWidthInTiles = mapWidthInTiles;
    }

    public override void Update(GameTime GameTime, List<GameObject> gameObjects)
    {
        GameObject meowBow = null;
        GameObject meowSword = null;

        foreach (var obj in gameObjects)
        {
            if (obj.Name == "MeowBow") meowBow = obj;
            else if (obj.Name == "MeowSword") meowSword = obj;
        }

        if (meowBow == null || meowSword == null || _mapWidthInTiles == 0)
            return;

        Vector2 midpoint = (meowBow.Position + meowSword.Position) / 2f;
        float desiredX = midpoint.X - _screenWidth / 2f;
        float maxX = (_mapWidthInTiles * _tileSize) - _screenWidth;
        CameraX = MathHelper.Clamp(desiredX, 0, maxX);
        Transform = Matrix.CreateTranslation(new Vector3(-CameraX, 0f, 0f));
    }
}