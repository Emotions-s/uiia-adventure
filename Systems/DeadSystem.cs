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
                obj.RemoveComponent<DeathFlagComponent>();
            }
        }
    }
}