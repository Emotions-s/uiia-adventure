using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using uiia_adventure.Components;
using uiia_adventure.Core;
using uiia_adventure.Globals;

namespace uiia_adventure.Systems;

public class CollisionSystem : SystemBase
{
    private List<(float, Rectangle)> _groundCollisionsCheckOrder = [];
    private List<(float, Rectangle)> _wallCollisionsCheckOrder = [];
    public override void Update(GameTime gameTime, List<GameObject> gameObjects)
    {
        float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
        foreach (var obj in gameObjects)
        {
            var physics = obj.GetComponent<PhysicsComponent>();
            var sprite = obj.GetComponent<SpriteComponent>();
            var debug = obj.GetComponent<DebugComponent>();
            if (physics == null || sprite == null || debug == null) continue;




            var ground = gameObjects.FirstOrDefault(obj => obj.GetComponent<GroundTileComponent>() != null)?.GetComponent<GroundTileComponent>();
            var wall = gameObjects.FirstOrDefault(obj => obj.GetComponent<WallTileComponent>() != null)?.GetComponent<WallTileComponent>();

            int TileSize = GameConstants.TileSize;
            Point posTileBotLeft = new((int)obj.Position.X / TileSize, (int)((obj.Position.Y + TileSize - 1) / TileSize));
            Point posTileBotRight = new((int)((obj.Position.X + TileSize - 1) / TileSize), (int)((obj.Position.Y + TileSize - 1) / TileSize));

            // check tile bot left and right if empty == apply gravity
            if (!ground.Tiles.Contains(posTileBotLeft) && !ground.Tiles.Contains(posTileBotRight))
            {
                physics.IsGrounded = false;
            }

            Rectangle playerRect = new(
                (int)obj.Position.X + sprite.SourceRect.X,
                (int)obj.Position.Y + sprite.SourceRect.Y,
                sprite.SourceRect.Width,
                sprite.SourceRect.Height
            );

            debug.PlayerTiles.Clear();
            HashSet<Point> visited = new();

            int left = playerRect.Left / GameConstants.TileSize;
            int right = (playerRect.Right - 1) / GameConstants.TileSize;
            int top = playerRect.Top / GameConstants.TileSize;
            int bottom = (playerRect.Bottom - 1) / GameConstants.TileSize;

            _groundCollisionsCheckOrder.Clear();
            _wallCollisionsCheckOrder.Clear();

            for (int x = left - 1; x <= right + 1; x++)
            {
                for (int y = top - 1; y <= bottom + 1; y++)
                {
                    Point gTile = new(x, y);
                    if (visited.Contains(gTile)) continue;
                    visited.Add(gTile);
                    debug.PlayerTiles.Add(gTile);

                    if (!ground.Tiles.Contains(gTile) && !wall.Tiles.Contains(gTile))
                    {
                        continue;
                    }

                    Rectangle tileRect = new(
                        gTile.X * GameConstants.TileSize,
                        gTile.Y * GameConstants.TileSize,
                        GameConstants.TileSize,
                        GameConstants.TileSize
                    );

                    // Get collision tiles and sort them by T distance
                    if (CollisionHelper.DynamicRectVsRect(playerRect, physics.Velocity, dt, tileRect,
                        out Vector2 contactPoint, out Vector2 contactNormal, out float contactTime))
                    {
                        if (ground.Tiles.Contains(gTile))
                            _groundCollisionsCheckOrder.Add((contactTime, tileRect));
                        else if (wall.Tiles.Contains(gTile))
                            _wallCollisionsCheckOrder.Add((contactTime, tileRect));
                    }
                }
            }
            // sort by distance
            _groundCollisionsCheckOrder.Sort((a, b) => a.Item1.CompareTo(b.Item1));
            _wallCollisionsCheckOrder.Sort((a, b) => a.Item1.CompareTo(b.Item1));

            foreach (var tileCollision in _wallCollisionsCheckOrder)
            {
                Rectangle tileCollisionRect = tileCollision.Item2;
                if (CollisionHelper.DynamicRectVsRect(playerRect, physics.Velocity, dt, tileCollisionRect,
                    out Vector2 contactPoint, out Vector2 contactNormal, out float contactTime))
                    physics.Velocity += contactNormal * new Vector2(Math.Abs(physics.Velocity.X), Math.Abs(physics.Velocity.Y)) * (1 - contactTime);

            }

            foreach (var tileCollision in _groundCollisionsCheckOrder)
            {
                Rectangle tileCollisionRect = tileCollision.Item2;
                if (CollisionHelper.DynamicRectVsRect(playerRect, physics.Velocity, dt, tileCollisionRect,
                    out Vector2 contactPoint, out Vector2 contactNormal, out float contactTime))
                {
                    physics.Velocity += contactNormal * new Vector2(Math.Abs(physics.Velocity.X), Math.Abs(physics.Velocity.Y)) * (1 - contactTime);
                    if (contactNormal.Y < 0)
                    {
                        physics.IsGrounded = true;
                    }

                }
            }
        }
    }
}