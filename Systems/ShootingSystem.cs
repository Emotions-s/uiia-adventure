namespace uiia_adventure.Systems;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using uiia_adventure.Core;
using uiia_adventure.Components;
using uiia_adventure.Factories;
using System;

public class ShootingSystem : SystemBase
{


    public override void Update(GameTime gameTime, List<GameObject> gameObjects)
    {
        GameObject arrow = null;

        var dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

        foreach (var shooter in gameObjects)
        {
            if (!shooter.HasComponent<CanShootComponent>())
            {
                continue;
            }
            var input = shooter.GetComponent<InputComponent>();
            var canShoot = shooter.GetComponent<CanShootComponent>();
            if (canShoot == null)
            {
                continue;
            }
            canShoot.timeSinceLastShot += dt;

            if (canShoot.timeSinceLastShot < canShoot.shootCooldown)
                continue;

            if (input == null || !input.ActionPressed) continue;

            canShoot.timeSinceLastShot = 0f;

            // Clone the projectile
            arrow = ProjectileFactory.ArrowTemplate.Clone();
            arrow.Position = shooter.Position + new Vector2(48, 0); // offset

            var physics = arrow.GetComponent<PhysicsComponent>();
            var projectile = arrow.GetComponent<ProjectileComponent>();
            int direction = input.LastDirectionKeyPressed == input.Left ? -1 : 1;
            physics.Velocity = new Vector2(projectile.Speed * direction, 0);

        }
        if (arrow != null)
            gameObjects.Add(arrow);
    }
}