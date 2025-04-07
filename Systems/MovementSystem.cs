namespace uiia_adventure.Systems;

using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using uiia_adventure.Components;
using uiia_adventure.Core;
using uiia_adventure.Managers;

public class MovementSystem(CameraSystem cameraSystem) : SystemBase
{
    private readonly CameraSystem _cameraSystem = cameraSystem;

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

            float cameraX = _cameraSystem.CameraX;
            float screenWidth = ResolutionManager.VirtualWidth;
            float spriteWidth = sprite.SourceRect.Width;

            float minX = cameraX;                               // Left bound of camera
            float maxX = cameraX + screenWidth - spriteWidth;   // Right bound of camera

            // Clamp each character's X position
            obj.Position.X = MathHelper.Clamp(obj.Position.X, minX, maxX);
        }
    }
}