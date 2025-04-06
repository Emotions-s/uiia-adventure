namespace uiia_adventure.Systems;

using Microsoft.Xna.Framework;
using System.Collections.Generic;
using uiia_adventure.Components;
using uiia_adventure.Core;

public class PhysicsSystem : SystemBase
{
    private const float Gravity = 1500f;

    public override void Update(GameTime gameTime, List<GameObject> gameObjects)
    {
        float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

        foreach (var obj in gameObjects)
        {
            var physics = obj.GetComponent<PhysicsComponent>();
            if (physics == null) continue;

            obj.Position += physics.Velocity * dt;

            if (!physics.IsGrounded) {
                physics.Velocity.Y += Gravity * dt;
            }
            // System.Diagnostics.Debug.WriteLine($"Position.Y = {obj.Position.Y}, Velocity.Y = {physics.Velocity.Y}, IsGrounded = {physics.IsGrounded}");
        }
    }
}