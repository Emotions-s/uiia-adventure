namespace uiia_adventure.Systems;

using Microsoft.Xna.Framework;
using System.Collections.Generic;
using uiia_adventure.Components;
using uiia_adventure.Core;


public class SwordAttackSystem : SystemBase
{
    public override void Update(GameTime gameTime, List<GameObject> gameObjects)
    {
        float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

        foreach (var obj in gameObjects)
        {
            var input = obj.GetComponent<InputComponent>();
            var melee = obj.GetComponent<MeleeComponent>();
            var sprite = obj.GetComponent<SpriteComponent>();

            var debug = obj.GetComponent<DebugComponent>();

            if (input == null || melee == null || sprite == null || debug == null) continue;

            melee.TimeSinceLastAttack += dt;

            debug.Rectangles.Clear();

            if (melee.TimeSinceLastAttack >= melee.AttackCooldown && input.ActionPressed)
            {
                // Reset cooldown
                melee.TimeSinceLastAttack = 0f;

                int direction = input.LastDirectionKeyPressed == input.Left ? -1 : 1;

                // Attack range rectangle (right-facing, you can adjust for direction later)
                Rectangle attackBox = new(
                    (int)obj.Position.X + sprite.SourceRect.Width * direction,
                    (int)obj.Position.Y,
                    (int)melee.Range,
                    sprite.SourceRect.Height
                );

                debug.Rectangles.Add(attackBox);

                foreach (var target in gameObjects)
                {
                    if (target == obj || !target.HasComponent<BreakableComponent>()) continue;

                    var targetSprite = target.GetComponent<SpriteComponent>();
                    if (targetSprite == null) continue;

                    Rectangle targetBox = new(
                        (int)target.Position.X,
                        (int)target.Position.Y,
                        targetSprite.SourceRect.Width,
                        targetSprite.SourceRect.Height
                    );

                    if (attackBox.Intersects(targetBox))
                    {
                        target.GetComponent<BreakableComponent>().Health--;
                    }
                }
            }
        }

        // Remove broken objects
        gameObjects.RemoveAll(go =>
            go.HasComponent<BreakableComponent>() &&
            go.GetComponent<BreakableComponent>().Health <= 0
        );
    }
}