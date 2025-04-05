using System.IO;
using GameNamespace.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using uiia_adventure.Factories;
using uiia_adventure.Globals;
using uiia_adventure.Managers;

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

        // var bowTex = Content.Load<Texture2D>("Characters/meow_bow");
        // var swordTex = Content.Load<Texture2D>("Characters/meow_sword");

        // create temp vector2 size 128 * 128 in color orange
        var bowTempTex = new Texture2D(GraphicsDevice, 128, 128);
        var data = new Color[128 * 128];
        for (int i = 0; i < data.Length; ++i) data[i] = Color.Orange;
        bowTempTex.SetData(data);

        var SwordTempTex = new Texture2D(GraphicsDevice, 128, 128);
        var data2 = new Color[128 * 128];
        for (int i = 0; i < data2.Length; ++i) data2[i] = Color.Gray;
        SwordTempTex.SetData(data2);

        CharacterManager characterManager = new();
        characterManager.Initialize(bowTempTex, SwordTempTex);

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
