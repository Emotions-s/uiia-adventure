namespace uiia_adventure.Managers;

using System;
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
    CutScene,
    Credits
}

public class SceneManager
{
    private SceneBase _currentScene;

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

    public void ChangeScene(LevelJsonModel levelData, SceneType type, SceneFlowController flowController)
    {

        switch (type)
        {
            case SceneType.Level:
                LevelScene levelScene = new(_graphics, _content, _spriteBatch, flowController);
                levelScene.SetPlayers(_characterManager.MeowBow, _characterManager.MeowSword);
                levelScene.Load(levelData);
                _currentScene = levelScene;
                break;
            case SceneType.CutScene:
                CutScene cutScene = new(_graphics, _content, _spriteBatch);
                cutScene.FlowController = flowController;
                _currentScene = cutScene;
                _currentScene.Load(levelData);
                break;
            case SceneType.Credits:
                var creditsScene = new CreditsScene(_graphics, _content, _spriteBatch);
                creditsScene.FlowController = flowController;
                _currentScene = creditsScene;
                _currentScene.Load(levelData);
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