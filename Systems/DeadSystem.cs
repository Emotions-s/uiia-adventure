namespace uiia_adventure.Systems;

using System.Collections.Generic;
using Microsoft.Xna.Framework;
using uiia_adventure.Core;
using uiia_adventure.Components;
using uiia_adventure.Managers;
public class DeathSystem : SystemBase
{
    public override void Update(GameTime gameTime, List<GameObject> gameObjects)
    {
        foreach (var obj in gameObjects)
        {
            if (obj.HasComponent<DeathFlagComponent>())
            {
                RespawnManager.RespawnPlayers(gameObjects);
                if (Game1.deathSound != null)
                {
                    Game1.deathSound.Play(1f, 0f, 0f);
                }
                RespawnManager.RespawnPlayers(gameObjects);
                obj.RemoveComponent<DeathFlagComponent>();
            }
        }
    }
}