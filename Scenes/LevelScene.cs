using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Diagnostics;
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

    public LevelScene(GraphicsDevice graphics, ContentManager content, SpriteBatch spriteBatch, SceneFlowController flowController)
    {
        FlowController = flowController;
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
        _updateSystems.Add(new ShootingSystem());
        _updateSystems.Add(new TurretSystem());
        _updateSystems.Add(new SwordAttackSystem());
        _updateSystems.Add(new ProjectileSystem());
        _updateSystems.Add(new ClimbSystem());
        _updateSystems.Add(new PushSystem());
        _updateSystems.Add(new CollisionSystem());
        _updateSystems.Add(new PhysicsSystem(_cameraSystem));
        _updateSystems.Add(new HazardSystem());
        _updateSystems.Add(new KeySystem());
        _updateSystems.Add(new ButtonSystem());
        _updateSystems.Add(new DoorSystem());
        _updateSystems.Add(new DeathSystem(FlowController));
        _updateSystems.Add(new FinishSystem(FlowController));

        _updateSystems.Add(new AnimationSystem());
        _updateSystems.Add(new DebugSkipSystem(FlowController));

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

    public override void Load(LevelJsonModel levelData)
    {
        // Load tile map and static elements
        var tileMapObject = _mapManager.LoadLevel(levelData, _content);
        _gameObjects.Add(tileMapObject);

        var tileMap = tileMapObject.GetComponent<TileMapComponent>();
        _cameraSystem.SetMapWidthInTiles(tileMap.TilesPerRowMap);

        // Add parallax background
        var bg1 = ResourceCache.GetTexture2D("map/background/background_1", _content);
        var bg2 = ResourceCache.GetTexture2D("map/background/background_2", _content);

        var parallaxObj = new GameObject
        {
            Name = "Parallax",
            Position = Vector2.Zero
        };
        parallaxObj.AddComponent(new ParallaxComponent(bg1, bg2, 0.05f, 0.1f));
        _gameObjects.Add(parallaxObj);

        // Add HUD texture refs (optional)
        hud1 = ResourceCache.GetTexture2D("map/HUD/Sword_Key", _content);
        hud2 = ResourceCache.GetTexture2D("map/HUD/Bow_Key", _content);

        _meowBow.Position = new Vector2(levelData.SpawnPoint[0], levelData.SpawnPoint[1]);
        _meowSword.Position = new Vector2(levelData.SpawnPoint[0] + GameConstants.TileSize, levelData.SpawnPoint[1]);

        // Add music player
        var musicPlayer = new GameObject { Name = "BackgroundMusic" };
        musicPlayer.AddComponent(new SoundComponent
        {
            BackgroundMusic = _content.Load<Microsoft.Xna.Framework.Media.Song>("audio/theme"),
            Volume = 0.1f,
            PlayMusicOnStart = true
        });
        _gameObjects.Add(musicPlayer);

        // Add objects from JSON data
        foreach (var objData in levelData.Objects)
        {
            var obj = ObjectFactory.CreateFrom(objData, _content);
            if (obj != null)
            {
                _gameObjects.Add(obj);
            }
        }

        // add finish comp
        var finishObj = new GameObject
        {
            Name = "Finish",
        };
        finishObj.AddComponent(new FinishTileComponent()
        {
            Area = new Rectangle(levelData.FinishPoint[0], levelData.FinishPoint[1], GameConstants.TileSize, GameConstants.TileSize),
        });
        _gameObjects.Add(finishObj);

        // keys
        foreach (var key in levelData.Keys)
        {
            var p = ObjectFactory.ParsePoint(key);
            var keyObj = new GameObject
            {
                Name = "Key",
                Position = new Vector2(p.X, p.Y),
            };
            keyObj.AddComponent(new KeyComponent());
            keyObj.AddComponent(new SpriteComponent()
            {
                Texture = ResourceCache.GetTexture2D("sprite/key", _content),
                SourceRect = new Rectangle(0, 0, GameConstants.TileSize, GameConstants.TileSize),
                RenderSource = new Rectangle(0, 0, GameConstants.TileSize, GameConstants.TileSize),
            });
            _gameObjects.Add(keyObj);
        }

        // keys inventory
        var keyInventory = new GameObject
        {
            Name = "KeyInventory",
            Position = new Vector2(0, 0),
        };
        keyInventory.AddComponent(new KeyInventoryComponent()
        {
            HaveToCollect = levelData.Keys.Count,
        });
        _gameObjects.Add(keyInventory);

        // doors
        var doorObj = new GameObject
        {
            Name = "Door",
            Position = new Vector2(levelData.FinishPoint[0], levelData.FinishPoint[1]),
        };
        doorObj.AddComponent(new DoorComponent());
        doorObj.AddComponent(new SpriteComponent()
        {
            Texture = ResourceCache.GetTexture2D("sprite/door", _content),
            SourceRect = new Rectangle(0, 0, GameConstants.TileSize, GameConstants.TileSize),
            RenderSource = new Rectangle(0, 0, GameConstants.TileSize, GameConstants.TileSize),
        });
        _gameObjects.Add(doorObj);

        // Add player characters
        _gameObjects.Add(_meowBow);
        _gameObjects.Add(_meowSword);
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