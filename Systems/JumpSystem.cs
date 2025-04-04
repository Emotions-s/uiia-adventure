using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using uiia_adventure.Components;
using uiia_adventure.Core;

namespace uiia_adventure.Systems;

public class JumpSystem : SystemBase
{
    private const float JumpCooldown = 0.2f;

    public override void Update(GameTime gameTime, List<GameObject> gameObjects)
    {
        float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

        foreach (var obj in gameObjects)
        {
            var input = obj.GetComponent<InputComponent>();
            var physics = obj.GetComponent<PhysicsComponent>();
            var jump = obj.GetComponent<JumpComponent>();

            if (input == null || physics == null || jump == null)
                continue;

            if (jump.CooldownTimer > 0f)
                jump.CooldownTimer -= dt;

            if (input.WantsToJump && physics.IsGrounded && jump.CooldownTimer <= 0f)
            {
                jump.IsJumping = true;
                jump.JumpStartY = 0f;
                physics.Velocity.Y = -jump.JumpSpeed;
                physics.IsGrounded = false;
                jump.CooldownTimer = JumpCooldown;
                jump.JumpStartY = obj.Position.Y;
            }

            float jumpHeightSoFar = jump.JumpStartY - obj.Position.Y;
            if (jump.IsJumping && input.WantsToJump && jump.MaxJumpHeight > jumpHeightSoFar)
            {
                physics.Velocity.Y -= jump.JumpSpeed * dt;
            }
            else if (jump.IsJumping)
            {
                jump.IsJumping = false;
            }
        }
    }
}
