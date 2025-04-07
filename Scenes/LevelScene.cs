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
        _updateSystems.Add(new DeathSystem());

        _updateSystems.Add(new AnimationSystem());

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
        _graphics.Clear(Color.CornflowerBlue);
        _spriteBatch.Begin(transformMatrix: _cameraSystem.Transform, samplerState: SamplerState.PointClamp);
        foreach (var system in _renderSystems)
            system.Update(gameTime, _gameObjects);

        _spriteBatch.End();
    }
}