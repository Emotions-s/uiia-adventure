namespace uiia_adventure.Systems;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using uiia_adventure.Components;
using uiia_adventure.Core;
using uiia_adventure.Globals;

public class DebugRenderSystem(SpriteBatch spriteBatch) : SystemBase
{
    private readonly SpriteBatch _spriteBatch = spriteBatch;
    public override void Update(GameTime gameTime, List<GameObject> gameObjects)
    {
        RenderGroundCollision(gameObjects);
        RenderWallCollision(gameObjects);
        RenderDebugTile(gameObjects);
    }

    private void DrawTile(Point tile, Color color)
    {
        var tileSize = new Vector2(GameConstants.TileSize, GameConstants.TileSize);
        var position = new Vector2(tile.X * tileSize.X, tile.Y * tileSize.Y);

        _spriteBatch.Draw(
            GameConstants.Pixel,
            position,
            null,
            color,
            0f,
            Vector2.Zero,
            tileSize,
            SpriteEffects.None,
            0f
        );
    }

    public void RenderGroundCollision(List<GameObject> gameObjects)
    {
        foreach (GameObject obj in gameObjects)
        {
            var groundTile = obj.GetComponent<GroundTileComponent>();
            if (groundTile == null)
                continue;

            foreach (var tile in groundTile.Tiles)
            {
                DrawTile(tile, new Color(255, 0, 0, 64));
            }
        }
    }

    public void RenderWallCollision(List<GameObject> gameObjects)
    {
        foreach (GameObject obj in gameObjects)
        {
            var wallTile = obj.GetComponent<WallTileComponent>();
            if (wallTile == null)
                continue;

            foreach (var tile in wallTile.Tiles)
            {
                DrawTile(tile, new Color(255, 165, 0, 64));

            }
        }
    }

    public void RenderDebugTile(List<GameObject> gameObjects)
    {
        foreach (var gameObject in gameObjects)
        {
            if (gameObject.HasComponent<DebugComponent>())
            {
                var debugComponent = gameObject.GetComponent<DebugComponent>();
                foreach (var tile in debugComponent.PlayerTiles)
                {
                    DrawTile(tile, new Color(0, 100, 0, 25));
                }
            }
        }
    }
}