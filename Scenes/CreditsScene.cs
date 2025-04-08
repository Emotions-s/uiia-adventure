using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using uiia_adventure.Core;
using uiia_adventure.Globals;

namespace uiia_adventure.Scenes
{
    public class CreditsScene : SceneBase
    {
        private Texture2D _creditsImage;
        private float _scrollY;
        private float _scrollSpeed = 55f;

        private Song _music;
        private bool _musicStarted = false;

        private readonly ContentManager _content;
        private readonly GraphicsDevice _graphics;
        private readonly SpriteBatch _spriteBatch;

        public CreditsScene(GraphicsDevice graphics, ContentManager content, SpriteBatch spriteBatch)
        {
            _graphics = graphics;
            _content = content;
            _spriteBatch = spriteBatch;
        }

        public override void Load(LevelData levelData)
        {
            _creditsImage = _content.Load<Texture2D>("cutscenes/credits"); // credits.png
            _music = _content.Load<Song>("audio/credits_song");           // credits_theme.mp3

            _scrollY = _graphics.Viewport.Height; // Start from bottom
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Volume = 0.5f;
        }

        public override void Update(GameTime gameTime)
        {
            if (!_musicStarted)
            {
                MediaPlayer.Play(_music);
                _musicStarted = true;
            }

            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
            _scrollY -= _scrollSpeed * dt;
        }

        public override void Draw(GameTime gameTime)
        {
            _graphics.Clear(Color.Black);

            _spriteBatch.Begin(samplerState: SamplerState.PointClamp);
            _spriteBatch.Draw(_creditsImage, new Vector2(0, _scrollY), Color.White);
            _spriteBatch.End();
        }
    }
}
