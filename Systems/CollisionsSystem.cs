using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using uiia_adventure.Components;
using uiia_adventure.Core;
using uiia_adventure.Globals;

namespace uiia_adventure.Systems;

public class CollisionSystem : SystemBase
{
    public override void Update(GameTime gameTime, List<GameObject> gameObjects)
    {
        foreach (var obj in gameObjects)
        {
            var physics = obj.GetComponent<PhysicsComponent>();
            if (physics == null) continue;

            GroundTileComponent ground = null;
            foreach (var mapObj in gameObjects)
            {
                ground = mapObj.GetComponent<GroundTileComponent>();
                if (ground != null) break;
            }

            if (ground == null) continue;

            // int TileSize = GameConstants.TileSize;
            // // Corner tile positions
            // Point posTileTopLeft = new((int)obj.Position.X / TileSize, (int)obj.Position.Y / TileSize);
            // Point posTileTopRight = new((int)((obj.Position.X + TileSize - 1) / TileSize), (int)obj.Position.Y / TileSize);
            // Point posTileBotLeft = new((int)obj.Position.X / TileSize, (int)((obj.Position.Y + TileSize - 1) / TileSize));
            // Point posTileBotRight = new((int)((obj.Position.X + TileSize - 1) / TileSize), (int)((obj.Position.Y + TileSize - 1) / TileSize));

            // if (obj.Name == "MeowBow")
            // {
            //     // Console.WriteLine($"Player Position: {obj.Position}");
            //     // Console.WriteLine($"TopLeft: {posTileTopLeft}, TopRight: {posTileTopRight}, BotLeft: {posTileBotLeft}, BotRight: {posTileBotRight}");
            //     // Console.WriteLine($"Velocity: {physics.Velocity}");
            // }


            // Point posNextBotTileLeft = new((int)obj.Position.X / TileSize, (int)((obj.Position.Y + TileSize) / TileSize));
            // Point posNextBotTileRight = new((int)((obj.Position.X + TileSize - 1) / TileSize), (int)((obj.Position.Y + TileSize) / TileSize));

            // // ground check
            // if (ground.Tiles.Contains(posNextBotTileLeft) || ground.Tiles.Contains(posNextBotTileRight))
            // {
            //     obj.Position = new Vector2(obj.Position.X, posNextBotTileLeft.Y * TileSize - TileSize);
            //     physics.Velocity.Y = 0;
            //     physics.IsGrounded = true;
            // }
            // else
            // {
            //     physics.IsGrounded = false;
            // }

            // // top check
            // if (ground.Tiles.Contains(posTileTopLeft) || ground.Tiles.Contains(posTileTopRight))
            // {
            //     obj.Position = new Vector2(obj.Position.X, posTileTopLeft.Y * TileSize + TileSize);
            //     physics.Velocity.Y = 0;
            // }

            // posTileTopLeft = new((int)obj.Position.X / TileSize, (int)obj.Position.Y / TileSize);
            // posTileTopRight = new((int)((obj.Position.X + TileSize - 1) / TileSize), (int)obj.Position.Y / TileSize);
            // posTileBotLeft = new((int)obj.Position.X / TileSize, (int)((obj.Position.Y + TileSize - 1) / TileSize));
            // posTileBotRight = new((int)((obj.Position.X + TileSize - 1) / TileSize), (int)((obj.Position.Y + TileSize - 1) / TileSize));


            // right check
            // if (ground.Tiles.Contains(posTileTopRight) || ground.Tiles.Contains(posTileBotRight))
            // {
            //     if (obj.Name == "MeowBow")
            //     {
            //         // Console.WriteLine($"TopLeft: {posTileTopLeft}, TopRight: {posTileTopRight}, BotLeft: {posTileBotLeft}, BotRight: {posTileBotRight}");
            //         Console.WriteLine($"Bot Tile: {posTileBotRight}, BotLeft: {posTileBotLeft}");
            //     }
            //     obj.Position = new Vector2(posTileTopLeft.X * TileSize, obj.Position.Y);
            //     physics.Velocity.X = 0;
            // }


            List<Rectangle> intersectingTiles = new();

            SpriteComponent sprite = obj.GetComponent<SpriteComponent>();

            DebugComponent debug = obj.GetComponent<DebugComponent>();
            if (sprite == null || debug == null) continue;

            Rectangle target = new(
                (int)obj.Position.X + sprite.SourceRect.X,
                (int)obj.Position.Y + sprite.SourceRect.Y,
                sprite.SourceRect.Width,
                sprite.SourceRect.Height
            );

            debug.PlayerTiles.Clear();

            intersectingTiles = getIntersectingTileHorizontal(target);

            foreach (var rect in intersectingTiles)
            {
                if (ground.Tiles.Contains(new Point(rect.X, rect.Y)))
                {
                    Rectangle collisitonRect = new(
                        rect.X * GameConstants.TileSize,
                        rect.Y * GameConstants.TileSize,
                        GameConstants.TileSize,
                        GameConstants.TileSize
                    );

                    if (physics.Velocity.X > 0)
                    {
                        obj.Position = new Vector2(collisitonRect.Left - sprite.SourceRect.Width, obj.Position.Y);
                    }
                    else if (physics.Velocity.X < 0)
                    {
                        obj.Position = new Vector2(collisitonRect.Right, obj.Position.Y);
                    }
                }
                else
                {
                    debug.PlayerTiles.Add(rect);
                }
            }

            intersectingTiles = getIntersectingTileVertical(target);

            foreach (var rect in intersectingTiles)
            {
                if (ground.Tiles.Contains(new Point(rect.X, rect.Y)))
                {
                    Rectangle collisionRect = new(
                        rect.X * GameConstants.TileSize,
                        rect.Y * GameConstants.TileSize,
                        GameConstants.TileSize,
                        GameConstants.TileSize
                    );

                    if (!physics.IsGrounded && physics.Velocity.Y > 0)
                    {
                        Console.WriteLine($"Collision: {collisionRect}");
                        obj.Position = new Vector2(obj.Position.X, collisionRect.Top - sprite.SourceRect.Height);
                        physics.IsGrounded = true;
                    }
                    else if (!physics.IsGrounded && physics.Velocity.Y < 0)
                    {
                        obj.Position = new Vector2(obj.Position.X, collisionRect.Bottom);
                    }

                }
                else
                {
                    physics.IsGrounded = false;
                    debug.PlayerTiles.Add(rect);
                }
            }

        }
    }

    private List<Rectangle> getIntersectingTileHorizontal(Rectangle target)
    {
        List<Rectangle> intersectingTiles = new();

        int widthInTiles = (target.Width - (target.Width % GameConstants.TileSize)) / GameConstants.TileSize;
        int heightInTiles = (target.Height - (target.Height % GameConstants.TileSize)) / GameConstants.TileSize;

        for (int x = 0; x <= widthInTiles; x++)
        {
            for (int y = 0; y <= heightInTiles; y++)
            {
                intersectingTiles.Add(new Rectangle(
                    (target.X + x * GameConstants.TileSize) / GameConstants.TileSize,
                    (target.Y + y * (GameConstants.TileSize - 1)) / GameConstants.TileSize,
                    GameConstants.TileSize,
                    GameConstants.TileSize
                ));
            }
        }
        return intersectingTiles;
    }

    private List<Rectangle> getIntersectingTileVertical(Rectangle target)
    {
        List<Rectangle> intersectingTiles = new();

        int widthInTiles = (target.Width - (target.Width % GameConstants.TileSize)) / GameConstants.TileSize;
        int heightInTiles = (target.Height - (target.Height % GameConstants.TileSize)) / GameConstants.TileSize;

        for (int x = 0; x <= widthInTiles; x++)
        {
            for (int y = 0; y <= heightInTiles; y++)
            {
                intersectingTiles.Add(new Rectangle(
                    (target.X + x * (GameConstants.TileSize - 1)) / GameConstants.TileSize,
                    (target.Y + y * GameConstants.TileSize) / GameConstants.TileSize,
                    GameConstants.TileSize,
                    GameConstants.TileSize
                ));
            }
        }
        return intersectingTiles;
    }
}
