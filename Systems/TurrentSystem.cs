using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using uiia_adventure.Components;
using uiia_adventure.Core;
using uiia_adventure.Factories;

namespace uiia_adventure.Systems;

public class TurretSystem : SystemBase
{
    public override void Update(GameTime gameTime, List<GameObject> gameObjects)
    {
        float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

        GameObject fireball = null;

        foreach (var turret in gameObjects)
        {
            var canShoot = turret.GetComponent<CanShootComponent>();
            var turComp = turret.GetComponent<TurretComponent>();
            var sprite = turret.GetComponent<SpriteComponent>();

            if (canShoot == null || turComp == null || sprite == null)
                continue;

            canShoot.timeSinceLastShot += dt;

            if (canShoot.timeSinceLastShot < canShoot.shootCooldown)
                continue;

            canShoot.timeSinceLastShot = 0;
            int direction = turComp.Direction.X < 0 ? -1 : 1;

            fireball = ProjectileFactory.Create(
                "fireball",
                turret.Position + new Vector2(0, -8)
            );

            var physics = fireball.GetComponent<PhysicsComponent>();
            var projectile = fireball.GetComponent<ProjectileComponent>();
            var bulSprite = fireball.GetComponent<SpriteComponent>();
            bulSprite.FlipHorizontally = direction == -1;
            physics.Velocity = new Vector2(projectile.Speed * direction, 0);
        }
        if (fireball != null)
            gameObjects.Add(fireball);
    }
}