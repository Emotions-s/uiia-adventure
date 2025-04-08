using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;
using uiia_adventure.Components;
using uiia_adventure.Core;
using uiia_adventure.Globals;
using uiia_adventure.Managers;

namespace uiia_adventure.Systems;

public class PushSystem : SystemBase
{
    private bool CanMoveBox(Vector2 newPosition, SpriteComponent sprite, HashSet<Point> wallTiles)
    {
        Rectangle boxRect = new(
            (int)newPosition.X + sprite.SourceRect.X,
            (int)newPosition.Y + sprite.SourceRect.Y,
            sprite.SourceRect.Width,
            sprite.SourceRect.Height
        );

        int left = boxRect.Left / GameConstants.TileSize;
        int right = (boxRect.Right - 1) / GameConstants.TileSize;
        int top = boxRect.Top / GameConstants.TileSize;
        int bottom = (boxRect.Bottom - 1) / GameConstants.TileSize;

        for (int x = left; x <= right; x++)
            for (int y = top; y <= bottom; y++)
                if (wallTiles.Contains(new Point(x, y)))
                    return false;

        return true;
    }

    public override void Update(GameTime gameTime, List<GameObject> gameObjects)
    {
        float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

        var wall = gameObjects.FirstOrDefault(g => g.HasComponent<WallTileComponent>())
            ?.GetComponent<WallTileComponent>();
        if (wall == null) return;


        var pushables = gameObjects
            .Where(b => b.HasComponent<PushableComponent>() && b.HasComponent<SpriteComponent>())
            .ToList();

        var pushers = gameObjects
            .Where(p => p.HasComponent<CanPushComponent>() && p.HasComponent<PhysicsComponent>() && p.HasComponent<SpriteComponent>())
            .ToList();


        foreach (var box in pushables)
        {
            var boxSprite = box.GetComponent<SpriteComponent>();
            var pushable = box.GetComponent<PushableComponent>();

            foreach (var player in pushers)
            {
                var playerPhysics = player.GetComponent<PhysicsComponent>();

                if (!CollisionHelper.ObjectVsObject(player, box))
                    continue;

                float pushX = playerPhysics.Velocity.X * dt;
                if (pushX == 0)
                    continue;

                bool wrongSide =
                    (player.Position.X < box.Position.X && pushX < 0) ||
                    (player.Position.X > box.Position.X && pushX > 0);
                if (wrongSide) continue;

                Vector2 newBoxPos = box.Position + new Vector2(pushX, 0);
                pushable.CanBePushed = CanMoveBox(newBoxPos, boxSprite, wall.Tiles);

                if (pushable.CanBePushed) {
                    box.Position = newBoxPos;
                }
                else
                {
                    playerPhysics.Velocity.X = 0;
                }

                break;
            }
        }
    }
}