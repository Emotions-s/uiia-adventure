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
        _cameraSystem = new CameraSystem(ResolutionManager.VirtualWidth, ResolutionManager.VirtualHeight);
        // Order matters here
        _updateSystems.Add(_cameraSystem);
        _updateSystems.Add(new InputSystem());
        _updateSystems.Add(new MovementSystem(_cameraSystem));
        _updateSystems.Add(new JumpSystem());
        _updateSystems.Add(new CollisionSystem());
        _updateSystems.Add(new PhysicsSystem());

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
        var tileMap = tileMapObject.GetComponent<TileMapComponent>();
        _cameraSystem.SetMapWidthInTiles(tileMap.TilesPerRowMap);

        _gameObjects.Add(tileMapObject);

        _meowBow.Position = new Vector2(64, 700);
        _meowSword.Position = new Vector2(192, 700);

        _gameObjects.Add(_meowBow);
        _gameObjects.Add(_meowSword);
    }

    public override void Update(GameTime gameTime)
    {
        foreach (var system in _updateSystems)
        {
            system.Update(gameTime, _gameObjects);
        }
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