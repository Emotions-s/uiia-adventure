using Microsoft.Xna.Framework;
using System.Collections.Generic;
using uiia_adventure.Core;
using uiia_adventure.Globals;
using uiia_adventure.Managers;

namespace uiia_adventure.Systems;

public class CameraSystem : SystemBase
{
    private int _mapWidthInTiles;
    private readonly int _screenWidth;
    private readonly int _tileSize;

    public Matrix Transform { get; private set; }
    public float CameraX { get; private set; }

    public CameraSystem(int screenWidth)
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
            if (obj.Name == GameConstants.MeowBowName) meowBow = obj;
            else if (obj.Name == GameConstants.MeowSwordName) meowSword = obj;
        }

        if (meowBow == null || meowSword == null || _mapWidthInTiles == 0)
            return;

        var meowBowSprite = meowBow.GetComponent<SpriteComponent>();
        float midpointX = (meowBow.Position.X + meowSword.Position.X + meowBowSprite.SourceRect.Width) / 2f;

        float desiredX = midpointX - _screenWidth / 2f;
        float maxX = (_mapWidthInTiles * _tileSize) - _screenWidth;
        CameraX = MathHelper.Clamp(desiredX, 0, maxX);

        Transform = Matrix.CreateTranslation(new Vector3(-CameraX, 0f, 0f));
    }
}