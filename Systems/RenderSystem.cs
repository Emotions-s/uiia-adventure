namespace uiia_adventure.Systems;

using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using uiia_adventure.Core;

public class RenderSystem(SpriteBatch spriteBatch) : SystemBase
{
    private readonly SpriteBatch _spriteBatch = spriteBatch;

    public override void Update(GameTime gameTime, List<GameObject> gameObjects)
    {
        _spriteBatch.Begin();

        foreach (var obj in gameObjects)
        {
            if (!obj.HasComponent<SpriteComponent>())
                continue;
            var sprite = obj.GetComponent<SpriteComponent>();

            _spriteBatch.Draw(
                sprite.Texture,
                obj.Position + sprite.Offset,
                sprite.SourceRect,
                Color.White
            );
        }

        _spriteBatch.End();
    }
}