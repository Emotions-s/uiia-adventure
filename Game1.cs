using System.IO;
using GameNamespace.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using uiia_adventure.Factories;
using uiia_adventure.Globals;
using uiia_adventure.Managers;
using uiia_adventure.Systems;

namespace uiia_adventure;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    private SceneManager _sceneManager;

    private RenderTarget2D _renderTarget;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        _graphics.PreferredBackBufferWidth = 1280;
        _graphics.PreferredBackBufferHeight = 720;
        _graphics.ApplyChanges();
        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        var meowBowStandTexture = Content.Load<Texture2D>("animation/meawbow_stand");
        var meowSwordStandTexture = Content.Load<Texture2D>("animation/meawsword_stand"); // adjust if filename differs
        var meowSwordWalkTexture = Content.Load<Texture2D>("animation/meowsword_walk");
        var meowBowWalkTexture = Content.Load<Texture2D>("animation/meowbow_walk");
        var meowSwordJumpTexture = Content.Load<Texture2D>("animation/meowsword_jump");
        var meowBowJumpTexture = Content.Load<Texture2D>("animation/meowbow_jump");

        CharacterManager characterManager = new();
        characterManager.Initialize(meowBowStandTexture, meowSwordStandTexture, meowSwordWalkTexture, meowBowWalkTexture, meowSwordJumpTexture, meowBowJumpTexture);

        LevelConfig.Load(Content);

        _sceneManager = new SceneManager(GraphicsDevice, Content, _spriteBatch, characterManager);
        _sceneManager.ChangeScene(LevelConfig.GetLevelByName("1-2"), SceneType.Level);

        _renderTarget = new RenderTarget2D(GraphicsDevice, ResolutionManager.VirtualWidth, ResolutionManager.VirtualHeight);

        var pixel = new Texture2D(_spriteBatch.GraphicsDevice, 1, 1);
        pixel.SetData([Color.White]);
        GameConstants.Pixel = pixel;
    }

    protected override void Update(GameTime gameTime)
    {
        _sceneManager.Update(gameTime);
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.SetRenderTarget(_renderTarget);
        GraphicsDevice.Clear(Color.CornflowerBlue);

        _sceneManager.Draw(gameTime);

        GraphicsDevice.SetRenderTarget(null);

        GraphicsDevice.Clear(Color.Black);
        var dest = ResolutionManager.GetDestinationRectangle(GraphicsDevice);

        _spriteBatch.Begin(samplerState: SamplerState.PointClamp);
        _spriteBatch.Draw(_renderTarget, dest, Color.White);
        _spriteBatch.End();

        base.Draw(gameTime);
    }
}
