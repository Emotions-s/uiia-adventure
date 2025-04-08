using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using uiia_adventure.Core;
using uiia_adventure.Globals;

namespace uiia_adventure.Scenes
{
    public class CutScene : SceneBase
    {
        private Texture2D[] _pages = new Texture2D[0];
        private int _currentPage = 0;

        private readonly ContentManager _content;
        private readonly GraphicsDevice _graphics;
        private readonly SpriteBatch _spriteBatch;

        private KeyboardState _previousState;

        private float _transitionAlpha = 0f;
        private bool _isTransitioning = true;
        private bool _isFadingOut = false;
        private int _nextPageIndex = -1;

        private Texture2D _blackPixel;
        private const float TransitionSpeed = 1f;

        public CutScene(GraphicsDevice graphics, ContentManager content, SpriteBatch spriteBatch)
        {
            _graphics = graphics;
            _content = content;
            _spriteBatch = spriteBatch;

            _blackPixel = new Texture2D(_graphics, 1, 1);
            _blackPixel.SetData(new[] { Color.Black });
        }

        public override void Load(LevelJsonModel levelData)
        {
            if (levelData.LevelName == "Ending")
            {
                _pages =
                [
                    _content.Load<Texture2D>("cutscenes/ending")
                ];
            }
            else
            {
                _pages =
                [
                    _content.Load<Texture2D>("cutscenes/page1"),
                    _content.Load<Texture2D>("cutscenes/page2"),
                    _content.Load<Texture2D>("cutscenes/page3")
                ];
            }

            // Start with fade-in
            _transitionAlpha = 1f;
            _isTransitioning = true;
            _isFadingOut = false;
        }

        public override void Update(GameTime gameTime)
        {
            KeyboardState current = Keyboard.GetState();

            if (!_isTransitioning)
            {
                if (current.IsKeyDown(Keys.Right) && !_previousState.IsKeyDown(Keys.Right))
                {
                    if (_currentPage == _pages.Length - 1)
                    {
                        FlowController.GoToNextScene();
                        return;
                    }
                    int nextPage = MathHelper.Clamp(_currentPage + 1, 0, _pages.Length - 1);
                    if (nextPage != _currentPage)
                    {
                        _nextPageIndex = nextPage;
                        _isTransitioning = true;
                        _isFadingOut = true;
                        _transitionAlpha = 0f;
                    }
                }

                if (current.IsKeyDown(Keys.Left) && !_previousState.IsKeyDown(Keys.Left))
                {
                    _currentPage = MathHelper.Clamp(_currentPage - 1, 0, _pages.Length - 1);
                }
            }
            else
            {
                float delta = (float)gameTime.ElapsedGameTime.TotalSeconds * TransitionSpeed;

                if (_isFadingOut)
                {
                    _transitionAlpha += delta;
                    if (_transitionAlpha >= 1f)
                    {
                        _transitionAlpha = 1f;
                        _currentPage = _nextPageIndex;
                        _isFadingOut = false;
                    }
                }
                else
                {
                    _transitionAlpha -= delta;
                    if (_transitionAlpha <= 0f)
                    {
                        _transitionAlpha = 0f;
                        _isTransitioning = false;
                    }
                }
            }

            _previousState = current;
        }

        public override void Draw(GameTime gameTime)
        {
            _graphics.Clear(Color.Black);

            _spriteBatch.Begin(samplerState: SamplerState.PointClamp);
            if (_pages.Length > 0)
            {
                _spriteBatch.Draw(_pages[_currentPage], Vector2.Zero, Color.White);
            }

            if (_isTransitioning || _transitionAlpha > 0f)
            {
                Color overlay = new Color(0, 0, 0, _transitionAlpha);
                _spriteBatch.Draw(_blackPixel, new Rectangle(0, 0, _graphics.Viewport.Width, _graphics.Viewport.Height), overlay);
            }

            _spriteBatch.End();
        }
    }
}