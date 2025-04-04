namespace uiia_adventure.Managers;

using System.Collections.Generic;
using GameNamespace.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using uiia_adventure.Globals;
using uiia_adventure.Scenes;

public enum SceneType
{
    Level,
    CutScene
}

public class SceneManager
{
    private SceneBase _currentScene;
    private Dictionary<string, SceneBase> _loadedScenes = new();

    private GraphicsDevice _graphics;
    private ContentManager _content;
    private SpriteBatch _spriteBatch;

    private CharacterManager _characterManager;
    public SceneBase CurrentScene => _currentScene;

    public SceneManager(GraphicsDevice graphics, ContentManager content, SpriteBatch spriteBatch, CharacterManager characterManager)
    {
        _graphics = graphics;
        _content = content;
        _spriteBatch = spriteBatch;
        _characterManager = characterManager;
    }

    public void ChangeScene(LevelData levelData, SceneType type)
    {
        if (_loadedScenes.TryGetValue(levelData.levelName, out var scene))
        {
            _currentScene = scene;
            return;
        }

        switch (type)
        {
            case SceneType.Level:
                LevelScene levelScene = new(_graphics, _content, _spriteBatch);
                levelScene.SetPlayers(_characterManager.MeowBow, _characterManager.MeowSword);
                levelScene.Load(levelData);
                _currentScene = levelScene;
                _loadedScenes[levelData.levelName] = levelScene;
                break;
            case SceneType.CutScene:
                // Implement CutSceneScene in the future
                break;
        }
    }

    public void Update(GameTime gameTime)
    {
        _currentScene?.Update(gameTime);
    }

    public void Draw(GameTime gameTime)
    {
        _currentScene?.Draw(gameTime);
    }
}