namespace uiia_adventure.Systems;

using Microsoft.Xna.Framework;
using System.Collections.Generic;
using uiia_adventure.Components;
using uiia_adventure.Core;
using uiia_adventure.Globals;
using uiia_adventure.Managers;

public class PhysicsSystem : SystemBase
{
    private const float Gravity = 1500f;
    private readonly CameraSystem _cameraSystem;

    public PhysicsSystem(CameraSystem cameraSystem)
    {
        _cameraSystem = cameraSystem;
    }

    public override void Update(GameTime gameTime, List<GameObject> gameObjects)
    {
        float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

        foreach (var obj in gameObjects)
        {
            var physics = obj.GetComponent<PhysicsComponent>();
            var sprite = obj.GetComponent<SpriteComponent>();
            if (physics == null || sprite == null) continue;

            // Apply velocity to position
            obj.Position += physics.Velocity * dt;

            // Apply gravity
            if (!physics.IsGrounded)
                physics.Velocity.Y += Gravity * dt * physics.GravityScale;

            // Clamp X position after physics is applied
            if (obj.Name == GameConstants.MeowBowName || obj.Name == GameConstants.MeowSwordName)
            {
                float minX = _cameraSystem.CameraX;
                float maxX = _cameraSystem.CameraX + ResolutionManager.VirtualWidth - sprite.SourceRect.Width;
                obj.Position.X = MathHelper.Clamp(obj.Position.X, minX, maxX);
            }
        }
    }
}