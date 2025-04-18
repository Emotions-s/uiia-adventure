namespace uiia_adventure.Systems;

using System.Collections.Generic;
using Microsoft.Xna.Framework;
using uiia_adventure.Core;
using uiia_adventure.Components;
using System.Linq;
using uiia_adventure.Globals;

public class HazardSystem : SystemBase
{
    public override void Update(GameTime gameTime, List<GameObject> gameObjects)
    {

        foreach (var obj in gameObjects)
        {
            if (obj.Name != GameConstants.MeowBowName && obj.Name != GameConstants.MeowSwordName) continue;
            var physics = obj.GetComponent<PhysicsComponent>();
            var sprite = obj.GetComponent<SpriteComponent>();
            if (physics == null || sprite == null) continue;

            var hazardTileObj = gameObjects.FirstOrDefault(o => o.HasComponent<HazardTileComponent>());
            if (hazardTileObj == null) return;

            var hazardTiles = hazardTileObj.GetComponent<HazardTileComponent>();
            if (hazardTiles == null) return;

            Rectangle playerRect = new(
                (int)obj.Position.X + sprite.SourceRect.X,
                (int)obj.Position.Y + sprite.SourceRect.Y,
                sprite.SourceRect.Width,
                sprite.SourceRect.Height
            );

            int tileSize = GameConstants.TileSize;

            // Check 4 corners of the player rect
            var corners = new[]
            {
                new Point(playerRect.Left / tileSize, playerRect.Top / tileSize),
                new Point((playerRect.Right - 1) / tileSize, playerRect.Top / tileSize),
                new Point(playerRect.Left / tileSize, (playerRect.Bottom - 1) / tileSize),
                new Point((playerRect.Right - 1) / tileSize, (playerRect.Bottom - 1) / tileSize)
            };

            foreach (var corner in corners)
            {
                if (hazardTiles.Tiles.Contains(corner))
                {
                    if (!obj.HasComponent<DeathFlagComponent>())
                    {
                        obj.AddComponent(new DeathFlagComponent());
                    }
                    break;
                }
            }

        }
    }
}
