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
    private CharacterManager _characterManager;


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
        var shootTexture = Content.Load<Texture2D>("animation/shooting");
        var swordTexture = Content.Load<Texture2D>("animation/sword");

        JumpSound1 = Content.Load<SoundEffect>("audio/cat_jumping");
        JumpSound2 = Content.Load<SoundEffect>("audio/cat_jumping2");
        deathSound = Content.Load<SoundEffect>("audio/cat_die");


        _characterManager = new CharacterManager();
        _characterManager.Initialize(meowBowStandTexture, meowSwordStandTexture, meowSwordWalkTexture, meowBowWalkTexture, meowSwordJumpTexture, meowBowJumpTexture, shootTexture, swordTexture);

        // Create SceneManager
        _sceneManager = new SceneManager(GraphicsDevice, Content, _spriteBatch, _characterManager);

        var arrowTexture = Content.Load<Texture2D>("sprite/arrow");
        ProjectileFactory.Initialize(arrowTexture);


        _renderTarget = new RenderTarget2D(GraphicsDevice, ResolutionManager.VirtualWidth, ResolutionManager.VirtualHeight);

        var pixel = new Texture2D(_spriteBatch.GraphicsDevice, 1, 1);
        pixel.SetData([Color.White]);
        GameConstants.Pixel = pixel;

        // Load and start with the CutScene
        var firstScene = LevelJsonLoader.LoadFromFile("../../../Content/Data/Levels/test.json");
        _sceneManager.ChangeScene(firstScene, SceneType.Level);
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
