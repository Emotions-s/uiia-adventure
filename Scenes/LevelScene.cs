using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
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
        // Order matters here
        _updateSystems.Add(new InputSystem());
        _updateSystems.Add(new MovementSystem());
        _updateSystems.Add(new JumpSystem());
        _updateSystems.Add(new PhysicsSystem());
        _updateSystems.Add(new CollisionSystem());

        _renderSystems.Add(new TileRenderSystem(_spriteBatch));
        _renderSystems.Add(new RenderSystem(_spriteBatch));
        _renderSystems.Add(new DebugRenderSystem(_spriteBatch));
    }

    public void SetPlayers(GameObject bow, GameObject sword)
    {
        _meowBow = bow;
        _meowSword = sword;
    }

    public override void Load(LevelData levelData)
    {
        var tileMapObject = _mapManager.LoadLevel(levelData, _content);
        _gameObjects.Add(tileMapObject);

        _meowBow.Position = new Vector2(64, 700);
        _meowSword.Position = new Vector2(192, 700);

        _gameObjects.Add(_meowBow);
        _gameObjects.Add(_meowSword);
    }

    public override void Update(GameTime gameTime)
    {
        foreach (var system in _updateSystems)
            system.Update(gameTime, _gameObjects);
    }

    public override void Draw(GameTime gameTime)
    {
        _graphics.Clear(Color.CornflowerBlue);

        foreach (var system in _renderSystems)
            system.Update(gameTime, _gameObjects);
    }
}