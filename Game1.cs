namespace uiia_adventure;

using GameNamespace.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using uiia_adventure.Components;
using uiia_adventure.Core;
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
    public static SoundEffect finishSound;
    private CharacterManager _characterManager;
    private SceneFlowController _sceneFlowController;

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

        var meowBowStandTexture = ResourceCache.GetTexture2D("animation/meawbow_stand", Content);
        var meowSwordStandTexture = ResourceCache.GetTexture2D("animation/meawsword_stand", Content);
        var meowSwordWalkTexture = ResourceCache.GetTexture2D("animation/meowsword_walk", Content);
        var meowBowWalkTexture = ResourceCache.GetTexture2D("animation/meowbow_walk", Content);
        var meowSwordJumpTexture = ResourceCache.GetTexture2D("animation/meowsword_jump", Content);
        var meowBowJumpTexture = ResourceCache.GetTexture2D("animation/meowbow_jump", Content);
        var shootTexture = ResourceCache.GetTexture2D("animation/shooting", Content);
        var swordTexture = ResourceCache.GetTexture2D("animation/sword", Content);

        JumpSound1 = ResourceCache.GetSoundEffect("audio/cat_jumping", Content);
        JumpSound2 = ResourceCache.GetSoundEffect("audio/cat_jumping2", Content);
        deathSound = ResourceCache.GetSoundEffect("audio/cat_die", Content);
        finishSound = ResourceCache.GetSoundEffect("audio/success", Content);

        _characterManager = new CharacterManager();
        _characterManager.Initialize(meowBowStandTexture, meowSwordStandTexture, meowSwordWalkTexture, meowBowWalkTexture, meowSwordJumpTexture, meowBowJumpTexture, shootTexture, swordTexture);

        // Create SceneManager
        _sceneManager = new SceneManager(GraphicsDevice, Content, _spriteBatch, _characterManager);
        _sceneFlowController = new SceneFlowController(_sceneManager);

        var arrowProjectile = new GameObject();
        arrowProjectile.AddComponent(new ProjectileComponent()
        {
            Speed = 500f
        });
        arrowProjectile.AddComponent(new SpriteComponent()
        {
            Texture = ResourceCache.GetTexture2D("sprite/arrow", Content),
            SourceRect = new Rectangle(0, 0, 64, 64),
            RenderSource = new Rectangle(0, 0, 64, 64),
        });
        arrowProjectile.AddComponent(new PhysicsComponent()
        {
            IsGrounded = false,
            GravityScale = 0f,
        });

        ProjectileFactory.RegisterTemplate("arrow", arrowProjectile);

        var fireballProjectile = new GameObject();
        fireballProjectile.AddComponent(new ProjectileComponent(){
            IsFromPlayer = false,
        });
        fireballProjectile.AddComponent(new SpriteComponent()
        {
            Texture = ResourceCache.GetTexture2D("sprite/fireball", Content),
            SourceRect = new Rectangle(0, 0, 64, 64),
            RenderSource = new Rectangle(0, 0, 64, 64),
        });
        fireballProjectile.AddComponent(new PhysicsComponent()
        {
            IsGrounded = false,
            GravityScale = 0f,
        });
        ProjectileFactory.RegisterTemplate("fireball", fireballProjectile);

        _renderTarget = new RenderTarget2D(GraphicsDevice, ResolutionManager.VirtualWidth, ResolutionManager.VirtualHeight);

        var pixel = new Texture2D(_spriteBatch.GraphicsDevice, 1, 1);
        pixel.SetData([Color.White]);
        GameConstants.Pixel = pixel;

        _sceneFlowController.GoToNextScene();
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
