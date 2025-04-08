namespace uiia_adventure.Systems;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using uiia_adventure.Core;
using uiia_adventure.Components;
public class ProjectileSystem : SystemBase
{
    public override void Update(GameTime gameTime, List<GameObject> gameObjects)
    {
        float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

        for (int i = gameObjects.Count - 1; i >= 0; i--)
        {
            var obj = gameObjects[i];
            var proj = obj.GetComponent<ProjectileComponent>();
            var physics = obj.GetComponent<PhysicsComponent>();

            if (proj == null || physics == null) continue;

            obj.Position += physics.Velocity * dt;

            proj.Elapsed += dt;
            if (proj.Elapsed >= proj.Lifetime)
            {
                gameObjects.RemoveAt(i);
            }
        }
    }
}