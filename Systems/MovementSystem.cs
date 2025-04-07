namespace uiia_adventure.Systems;

using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using uiia_adventure.Components;
using uiia_adventure.Core;

public class MovementSystem : SystemBase
{
    public override void Update(GameTime gameTime, List<GameObject> gameObjects)
    {
        foreach (var obj in gameObjects)
        {
            var input = obj.GetComponent<InputComponent>();
            var physics = obj.GetComponent<PhysicsComponent>();
            var stats = obj.GetComponent<StatsComponent>();
            var sprite = obj.GetComponent<SpriteComponent>();

            if (input == null || physics == null || stats == null || sprite == null) continue;

            // Apply horizontal movement only
            physics.Velocity.X = input.MoveDirection * stats.MoveSpeed;
        }
    }
}