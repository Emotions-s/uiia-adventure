using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using uiia_adventure.Components;
using uiia_adventure.Core;

namespace uiia_adventure.Factories;

public static class ProjectileFactory
{
    public static GameObject ArrowTemplate { get; private set; }

    public static void Initialize(Texture2D arrowTexture)
    {
        var arrow = new GameObject();
        arrow.Name = "ArrowTemplate";

        arrow.AddComponent(new SpriteComponent
        {
            Texture = arrowTexture,
            SourceRect = new Rectangle(0, 0, 64, 64),
            RenderSource = new Rectangle(0, 0, 64, 64),
            Offset = Vector2.Zero
        });

        arrow.AddComponent(new PhysicsComponent
        {
            Velocity = Vector2.Zero,
            IsGrounded = false,
            GravityScale = 0f
        });

        arrow.AddComponent(new ProjectileComponent());

        ArrowTemplate = arrow;
    }
}