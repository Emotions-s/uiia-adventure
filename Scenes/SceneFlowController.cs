using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using uiia_adventure.Managers;

namespace uiia_adventure.Globals;

public class SceneFlowController
{
    private readonly SceneManager _sceneManager;
    private List<SceneFlowEntry> _sceneSequence = new();
    private int _currentIndex = -1;

    public SceneFlowController(SceneManager sceneManager)
    {
        _sceneManager = sceneManager;
        LoadSceneFlow();
    }

    private void LoadSceneFlow()
    {
        string path = "Data/game_flow.json";

        string basePath;

#if DEBUG
        basePath = Path.Combine(AppContext.BaseDirectory, "../../../Content");
#else
    basePath = Path.Combine(AppContext.BaseDirectory, "Content");
#endif

        string jsonPath = Path.Combine(basePath, path);

        var json = File.ReadAllText(jsonPath);

        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            Converters = { new System.Text.Json.Serialization.JsonStringEnumConverter(JsonNamingPolicy.CamelCase) }
        };

        _sceneSequence = JsonSerializer.Deserialize<List<SceneFlowEntry>>(json, options) ?? new();

        foreach (var entry in _sceneSequence)
        {
            Console.WriteLine($"Scene: {entry.Path} Type: {entry.SceneType}");
        }
    }

    public void GoToNextScene()
    {
        _currentIndex++;

        RestartScene();
    }

    public void RestartScene()
    {
        if (_currentIndex < 0 || _currentIndex >= _sceneSequence.Count)
            return;

        var current = _sceneSequence[_currentIndex];

        var levelData = LevelJsonLoader.LoadFromFile(current.Path);
        _sceneManager.ChangeScene(levelData, current.SceneType, this);
    }
}