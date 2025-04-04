using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using uiia_adventure.Components;
using uiia_adventure.Core;
using uiia_adventure.Systems;

public class MovementSystem : SystemBase
{
    public override void Update(GameTime gameTime, List<GameObject> gameObjects)
    {
        foreach (var obj in gameObjects)
        {
            var input = obj.GetComponent<InputComponent>();
            var physics = obj.GetComponent<PhysicsComponent>();
            var stats = obj.GetComponent<StatsComponent>();

            if (input == null || physics == null || stats == null) continue;

            // Apply horizontal movement only
            physics.Velocity.X = input.MoveDirection * stats.MoveSpeed;
        }
    }
}