using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using uiia_adventure.Components;

class SpriteComponent : IComponent
{
    public Texture2D Texture;
    public Rectangle SourceRect;
    public Rectangle RenderSource;

    public Vector2 Offset;
    public bool FlipHorizontally = false;

    public SpriteComponent()
    {
        Texture = null;
        SourceRect = Rectangle.Empty;
        Offset = Vector2.Zero;
    }

    public SpriteComponent(Texture2D texture, Rectangle sourceRect, Vector2 offset)
    {
        this.Texture = texture;
        this.SourceRect = sourceRect;
        this.Offset = offset;
    }
}