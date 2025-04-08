using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using uiia_adventure.Components;
using uiia_adventure.Core;
using uiia_adventure.Globals;
using uiia_adventure.Managers;
using uiia_adventure.Systems;

namespace uiia_adventure.Scenes;

public class LevelScene : SceneBase
{
    private readonly List<GameObject> _gameObjects = new();
    private readonly List<SystemBase> _updateSystems = new();
    private readonly List<SystemBase> _renderSystems = new();
    private CameraSystem _cameraSystem;
    private readonly MapManager _mapManager = new();
    private readonly GraphicsDevice _graphics;
    private readonly ContentManager _content;
    private readonly SpriteBatch _spriteBatch;
    private readonly SoundSystem _soundSystem = new();
    private Texture2D hud1;
    private Texture2D hud2;
    private GameObject _meowBow;
    private GameObject _meowSword;

    public LevelScene(GraphicsDevice graphics, ContentManager content, SpriteBatch spriteBatch)
    {
        _graphics = graphics;
        _content = content;
        _spriteBatch = spriteBatch;
        InitializeSystems();
    }

    private void InitializeSystems()
    {
        _cameraSystem = new CameraSystem(ResolutionManager.VirtualWidth);
        // Order matters here
        _updateSystems.Add(_cameraSystem);
        _updateSystems.Add(new InputSystem());
        _updateSystems.Add(new MovementSystem());
        _updateSystems.Add(new JumpSystem());
        _updateSystems.Add(new ClimbSystem());
        _updateSystems.Add(new CollisionSystem());
        _updateSystems.Add(new PhysicsSystem(_cameraSystem));
        _updateSystems.Add(new HazardSystem());
        _updateSystems.Add(new ButtonSystem());
        _updateSystems.Add(new DeathSystem());

        _updateSystems.Add(new AnimationSystem());

        // Parallax
        _renderSystems.Add(new ParallaxSystem(_spriteBatch, _cameraSystem));
        // Parallax
        _renderSystems.Add(new ParallaxSystem(_spriteBatch, _cameraSystem));
        _renderSystems.Add(new TileRenderSystem(_spriteBatch));
        _renderSystems.Add(new RenderSystem(_spriteBatch));

        // Uncomment this line to add the debug render system
        // _renderSystems.Add(new DebugRenderSystem(_spriteBatch));
    }

    public void SetPlayers(GameObject bow, GameObject sword)
    {
        _meowBow = bow;
        _meowSword = sword;
    }

    public override void Load(LevelData levelData)
    {
        var tileMapObject = _mapManager.LoadLevel(levelData, _content);
        RespawnManager.SpawnPoint = levelData.SpawnPoint;
        var tileMap = tileMapObject.GetComponent<TileMapComponent>();
        var musicPlayer = new GameObject();

        _cameraSystem.SetMapWidthInTiles(tileMap.TilesPerRowMap);

        _gameObjects.Add(tileMapObject);

        var bg1 = _content.Load<Texture2D>("map/background/background_1");
        var bg2 = _content.Load<Texture2D>("map/background/background_2");

        var parallax = new ParallaxComponent(bg1, bg2, 0.05f, 0.1f);
        var parallaxObj = new GameObject();
        parallaxObj.Position = Vector2.Zero;
        parallaxObj.AddComponent(parallax);
        _gameObjects.Add(parallaxObj);

        hud1 = _content.Load<Texture2D>("map/HUD/Sword_Key");
        hud2 = _content.Load<Texture2D>("map/HUD/Bow_Key");

        _meowBow.Position = new Vector2(64, 700);
        _meowSword.Position = new Vector2(192, 700);

        _gameObjects.Add(_meowBow);
        _gameObjects.Add(_meowSword);
        // Create music player object
        musicPlayer.Name = "BackgroundMusic";
        musicPlayer.AddComponent(new SoundComponent
        {
            BackgroundMusic = _content.Load<Microsoft.Xna.Framework.Media.Song>("audio/theme"),
            Volume = 0.1f,
            PlayMusicOnStart = true
        });

        _gameObjects.Add(musicPlayer);

        // load buttons
        var buttonObj = new GameObject();
        buttonObj.Name = "btn1-1-1";
        buttonObj.Position = new Vector2(64 * 11, 64 * 7);
        buttonObj.AddComponent(new SpriteComponent
        {
            Texture = _content.Load<Texture2D>("sprite/button"),
            SourceRect = new Rectangle(0, 0, 64, 64),
            RenderSource = new Rectangle(0, 0, 64, 64),
        });
        buttonObj.AddComponent(new ButtonComponent
        {
            TargetIds = new List<string> { "ladder1-1-1" },
        });

        var ladderObj = new GameObject();
        ladderObj.Name = "ladder1-1-1";
        ladderObj.AddComponent(new TriggerableTileMapComponent()
        {
            TriggerId = "ladder1-1-1",
            Grid = new Dictionary<Point, int>
            {
                { new Point(9, 8), 32 },
                { new Point(9, 9), 32 },
                { new Point(9, 10), 32 },
                { new Point(9, 11), 32 },
            }
        });
        _gameObjects.Add(buttonObj);
        _gameObjects.Add(ladderObj);

        RespawnManager.RespawnPlayers(_gameObjects);
    }

    public override void Update(GameTime gameTime)
    {
        foreach (var system in _updateSystems)
        {
            system.Update(gameTime, _gameObjects);
        }
        _soundSystem.Update(gameTime, _gameObjects);
    }

    public override void Draw(GameTime gameTime)
    {
        _graphics.Clear(new Color(26, 24, 24));
        _spriteBatch.Begin(transformMatrix: _cameraSystem.Transform, samplerState: SamplerState.PointClamp);
        

        foreach (var system in _renderSystems)
            system.Update(gameTime, _gameObjects);
        _spriteBatch.End();

        _spriteBatch.Begin(transformMatrix: null, samplerState: SamplerState.PointClamp);
        _spriteBatch.Draw(hud1, new Vector2(GameConstants.TileSize * 1, GameConstants.TileSize * 1), Color.White);
        _spriteBatch.Draw(hud2, new Vector2(GameConstants.TileSize * 19, GameConstants.TileSize * 1), Color.White);
        _spriteBatch.End();
    }
}