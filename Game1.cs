namespace uiia_adventure;

using GameNamespace.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using uiia_adventure.Factories;
using uiia_adventure.Globals;
using uiia_adventure.Managers;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    private SceneManager _sceneManager;

    private RenderTarget2D _renderTarget;
    public static SoundEffect JumpSound1;
    public static SoundEffect JumpSound2;
    public static SoundEffect deathSound;


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
        var meowSwordStandTexture = Content.Load<Texture2D>("animation/meawsword_stand");
        var meowSwordWalkTexture = Content.Load<Texture2D>("animation/meowsword_walk");
        var meowBowWalkTexture = Content.Load<Texture2D>("animation/meowbow_walk");
        var meowSwordJumpTexture = Content.Load<Texture2D>("animation/meowsword_jump");
        var meowBowJumpTexture = Content.Load<Texture2D>("animation/meowbow_jump");

        JumpSound1 = Content.Load<SoundEffect>("audio/cat_jumping");
        JumpSound2 = Content.Load<SoundEffect>("audio/cat_jumping2");
        deathSound = Content.Load<SoundEffect>("audio/cat_die");


        CharacterManager characterManager = new();
        characterManager.Initialize(meowBowStandTexture, meowSwordStandTexture, meowSwordWalkTexture, meowBowWalkTexture, meowSwordJumpTexture, meowBowJumpTexture);

        LevelConfig.Load(Content);

        _sceneManager = new SceneManager(GraphicsDevice, Content, _spriteBatch, characterManager);
        _sceneManager.ChangeScene(LevelConfig.GetLevelByName("test"), SceneType.Level);
        //cut scene
        //_sceneManager.ChangeScene(new LevelData { LevelName = "IntroCutscene" }, SceneType.CutScene);
        //_sceneManager.ChangeScene(new LevelData { LevelName = "Ending" }, SceneType.CutScene);
        //_sceneManager.ChangeScene(new LevelData { LevelName = "Credits" }, SceneType.Credits);


        var arrowTexture = Content.Load<Texture2D>("sprite/box");
        ProjectileFactory.Initialize(arrowTexture);


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
