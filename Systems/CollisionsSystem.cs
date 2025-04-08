using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        var pushableObjects = gameObjects.Where(g => g.HasComponent<PushableComponent>()).ToList();

        foreach (var obj in gameObjects)
        {
            var physics = obj.GetComponent<PhysicsComponent>();
            var sprite = obj.GetComponent<SpriteComponent>();
            var debug = obj.GetComponent<DebugComponent>();
            if (physics == null || sprite == null) continue;

            var ground = gameObjects.FirstOrDefault(obj => obj.GetComponent<GroundTileComponent>() != null)?.GetComponent<GroundTileComponent>();
            var wall = gameObjects.FirstOrDefault(obj => obj.GetComponent<WallTileComponent>() != null)?.GetComponent<WallTileComponent>();

            var projectile = obj.GetComponent<ProjectileComponent>();

            Rectangle currentRect = new(
                (int)obj.Position.X + sprite.SourceRect.X,
                (int)obj.Position.Y + sprite.SourceRect.Y,
                sprite.SourceRect.Width,
                sprite.SourceRect.Height
            );

            int TileSize = GameConstants.TileSize;
            Point posTileBotLeft = new((int)obj.Position.X / TileSize, (int)((obj.Position.Y + TileSize) / TileSize));
            Point posTileBotRight = new((int)((obj.Position.X + TileSize - 1) / TileSize), (int)((obj.Position.Y + TileSize) / TileSize));

            // check tile bot left and right if empty == apply gravity
            if (!ground.Tiles.Contains(posTileBotLeft) && !ground.Tiles.Contains(posTileBotRight))
            {
                if (obj.Name == GameConstants.MeowBowName || obj.Name == GameConstants.MeowSwordName)
                {
                    bool isOnBox = false;
                    foreach (var box in pushableObjects)
                    {
                        var boxSprite = box.GetComponent<SpriteComponent>();
                        if (boxSprite != null && IsStandingOnBox(currentRect, box, boxSprite))
                        {
                            isOnBox = true;
                            break;
                        }
                    }
                    physics.IsGrounded = isOnBox;
                }
                else
                {
                    physics.IsGrounded = false;
                }
            }


            if (debug != null)
                debug.PlayerTiles.Clear();
            HashSet<Point> visited = new();

            int left = currentRect.Left / GameConstants.TileSize;
            int right = (currentRect.Right - 1) / GameConstants.TileSize;
            int top = currentRect.Top / GameConstants.TileSize;
            int bottom = (currentRect.Bottom - 1) / GameConstants.TileSize;

            _groundCollisionsCheckOrder.Clear();
            _wallCollisionsCheckOrder.Clear();

            for (int x = left - 1; x <= right + 1; x++)
            {
                for (int y = top - 1; y <= bottom + 1; y++)
                {
                    Point gTile = new(x, y);
                    if (visited.Contains(gTile)) continue;
                    visited.Add(gTile);
                    if (debug != null)
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
                    if (CollisionHelper.DynamicRectVsRect(currentRect, physics.Velocity, dt, tileRect,
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

            // check walls
            foreach (var tileCollision in _wallCollisionsCheckOrder)
            {
                Rectangle tileCollisionRect = tileCollision.Item2;
                if (CollisionHelper.DynamicRectVsRect(currentRect, physics.Velocity, dt, tileCollisionRect,
                    out Vector2 contactPoint, out Vector2 contactNormal, out float contactTime))
                    physics.Velocity += contactNormal * new Vector2(Math.Abs(physics.Velocity.X), Math.Abs(physics.Velocity.Y)) * (1 - contactTime);
                if (projectile != null)
                {
                    projectile.IsBlocked = true;
                }

            }

            // check ground
            foreach (var tileCollision in _groundCollisionsCheckOrder)
            {
                Rectangle tileCollisionRect = tileCollision.Item2;
                if (CollisionHelper.DynamicRectVsRect(currentRect, physics.Velocity, dt, tileCollisionRect,
                    out Vector2 contactPoint, out Vector2 contactNormal, out float contactTime))
                {
                    physics.Velocity += contactNormal * new Vector2(Math.Abs(physics.Velocity.X), Math.Abs(physics.Velocity.Y)) * (1 - contactTime);
                    if (contactNormal.Y < 0)
                    {
                        physics.IsGrounded = true;
                    }
                    if (projectile != null)
                    {
                        projectile.IsBlocked = true;
                    }

                }
            }

            // check pushable objects
            var playerPushable = obj.GetComponent<CanPushComponent>();

            foreach (var box in pushableObjects)
            {
                var boxSprite = box.GetComponent<SpriteComponent>();
                var pushablesComponent = box.GetComponent<PushableComponent>();
                if (boxSprite == null) continue;

                Rectangle boxRect = new(
                    (int)box.Position.X + boxSprite.SourceRect.X,
                    (int)box.Position.Y + boxSprite.SourceRect.Y,
                    boxSprite.SourceRect.Width,
                    boxSprite.SourceRect.Height
                );

                if (CollisionHelper.DynamicRectVsRect(currentRect, physics.Velocity, dt, boxRect,
                    out Vector2 contactPoint, out Vector2 contactNormal, out float contactTime))
                {
                    if (playerPushable == null || !pushablesComponent.CanBePushed || contactNormal.Y < 0)
                    {
                        physics.Velocity += contactNormal * new Vector2(Math.Abs(physics.Velocity.X), Math.Abs(physics.Velocity.Y)) * (1 - contactTime);
                    }
                    if (contactNormal.Y < 0)
                        physics.IsGrounded = true;
                    if (projectile != null)
                    {
                        projectile.IsBlocked = true;
                    }
                }
            }

            // check breakable objects
            var breakables = gameObjects.Where(o => o.HasComponent<BreakableComponent>());

            foreach (var breakable in breakables)
            {
                var breakableData = breakable.GetComponent<BreakableComponent>();
                if (breakableData.Health <= 0) continue;

                var breakableSprite = breakable.GetComponent<SpriteComponent>();
                if (breakableSprite == null) continue;

                Rectangle breakableRect = new(
                    (int)breakable.Position.X + breakableSprite.SourceRect.X,
                    (int)breakable.Position.Y + breakableSprite.SourceRect.Y,
                    breakableSprite.SourceRect.Width,
                    breakableSprite.SourceRect.Height
                );

                if (CollisionHelper.DynamicRectVsRect(currentRect, physics.Velocity, dt, breakableRect,
                    out Vector2 contactPoint, out Vector2 contactNormal, out float contactTime))
                {
                    physics.Velocity += contactNormal * new Vector2(Math.Abs(physics.Velocity.X), Math.Abs(physics.Velocity.Y)) * (1 - contactTime);
                    if (contactNormal.Y < 0)
                        physics.IsGrounded = true;
                    if (projectile != null)
                    {
                        projectile.IsBlocked = true;
                    }
                }
            }
        }
    }
    private static bool IsStandingOnBox(Rectangle playerRect, GameObject box, SpriteComponent boxSprite)
    {
        Rectangle boxRect = new(
            (int)box.Position.X + boxSprite.SourceRect.X,
            (int)box.Position.Y + boxSprite.SourceRect.Y,
            boxSprite.SourceRect.Width,
            boxSprite.SourceRect.Height
        );

        // Only consider "standing on" if bottom of player intersects top of box
        Rectangle feet = new(playerRect.Left, playerRect.Bottom, playerRect.Width, 1);
        Rectangle topOfBox = new(boxRect.Left, boxRect.Top - 1, boxRect.Width, 2);

        return feet.Intersects(topOfBox);
    }
}