namespace uiia_adventure.Systems;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using uiia_adventure.Core;
using uiia_adventure.Components;
using uiia_adventure.Globals;

public class ProjectileSystem : SystemBase
{
    public override void Update(GameTime gameTime, List<GameObject> gameObjects)
    {
        float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

        List<GameObject> projectilesToRemove = new();

        foreach (var obj in gameObjects)
        {
            var proj = obj.GetComponent<ProjectileComponent>();
            var physics = obj.GetComponent<PhysicsComponent>();

            if (proj == null || physics == null) continue;

            proj.Elapsed += dt;
            if (proj.IsBlocked || proj.Elapsed >= proj.Lifetime)
            {
                projectilesToRemove.Add(obj);
            }
            if (!proj.IsFromPlayer)
            {
                var sprite = obj.GetComponent<SpriteComponent>();
                var players = SystemHelper.GetPlayerGameObjects(gameObjects);
                foreach (var player in players)
                {
                    var playerSprite = player.GetComponent<SpriteComponent>();
                    if (playerSprite == null) continue;

                    Rectangle projRect = new(
                        (int)obj.Position.X + sprite.SourceRect.X,
                        (int)obj.Position.Y + sprite.SourceRect.Y,
                        sprite.SourceRect.Width,
                        sprite.SourceRect.Height
                    );

                    Rectangle playerRect = new(
                        (int)player.Position.X + playerSprite.SourceRect.X,
                        (int)player.Position.Y + playerSprite.SourceRect.Y,
                        playerSprite.SourceRect.Width,
                        playerSprite.SourceRect.Height
                    );

                    if (projRect.Intersects(playerRect))
                    {
                        if (!player.HasComponent<DeathFlagComponent>())
                        {
                            player.AddComponent(new DeathFlagComponent());
                        }
                        projectilesToRemove.Add(obj);
                        break;
                    }
                }
            }
        }
        foreach (var obj in projectilesToRemove)
        {
            gameObjects.Remove(obj);
        }
    }
}