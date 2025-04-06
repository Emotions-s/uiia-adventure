// AnimationSystem.cs
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using uiia_adventure.Components;
using uiia_adventure.Core;

namespace uiia_adventure.Systems
{
    public class AnimationSystem : SystemBase
    {
        public override void Update(GameTime gameTime, List<GameObject> gameObjects)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

            foreach (var obj in gameObjects)
            {
                var sprite = obj.GetComponent<SpriteComponent>();
                var input = obj.GetComponent<InputComponent>();
                var walkAnim = obj.GetComponent<WalkAnimationComponent>();

                if (sprite == null || input == null || walkAnim == null) continue;

                // Flip based on actual key
                if (input.LastDirectionKeyPressed == input.Right)
                    sprite.FlipHorizontally = false;
                else if (input.LastDirectionKeyPressed == input.Left)
                    sprite.FlipHorizontally = true;

                walkAnim.MoveDirection = input.MoveDirection;

                if (walkAnim.IsWalking)
                {
                    sprite.Texture = walkAnim.WalkTexture;

                    walkAnim.FrameTimer += dt;
                    if (walkAnim.FrameTimer >= walkAnim.FrameSpeed)
                    {
                        walkAnim.FrameTimer = 0f;
                        walkAnim.CurrentFrame = (walkAnim.CurrentFrame + 1) % walkAnim.Frames.Count;
                    }

                    sprite.RenderSource = walkAnim.Frames[walkAnim.CurrentFrame];
                }
                else
                {
                    sprite.Texture = walkAnim.IdleTexture;
                    walkAnim.CurrentFrame = 0;
                    if (walkAnim.Frames.Count > 0)
                        sprite.RenderSource = walkAnim.Frames[0];
                }
            }
        }
    }
}