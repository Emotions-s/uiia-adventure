namespace uiia_adventure.Systems;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using uiia_adventure.Components;
using uiia_adventure.Core;

public class ParallaxSystem : SystemBase
{
    private readonly SpriteBatch _spriteBatch;
    private readonly CameraSystem _cameraSystem;


    public ParallaxSystem(SpriteBatch spriteBatch, CameraSystem cameraSystem)
    {
        _spriteBatch = spriteBatch;
        _cameraSystem = cameraSystem;
    }

    public override void Update(GameTime gameTime, List<GameObject> gameObjects)
    {
        foreach (var obj in gameObjects)
        {
            if (obj.HasComponent<ParallaxComponent>())
            {
                var parallax = obj.GetComponent<ParallaxComponent>();
                float cameraX = _cameraSystem.CameraX;
                float parallaxX1 = cameraX * parallax.ParallaxFactor1;
                float parallaxX2 = cameraX * parallax.ParallaxFactor2;

                // Render Background1 (layer 1)
                _spriteBatch.Draw(parallax.Background1, new Vector2(-parallaxX1, 0), Color.White);
                _spriteBatch.Draw(parallax.Background1, new Vector2(parallax.Background1.Width - parallaxX1, 0), Color.White);

                // Render Background2 (layer 2)
                _spriteBatch.Draw(parallax.Background2, new Vector2(-parallaxX2, 0), Color.White);
                _spriteBatch.Draw(parallax.Background2, new Vector2(parallax.Background2.Width - parallaxX2, 0), Color.White);
            }
        }
    }
    
}
