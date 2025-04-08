using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using uiia_adventure.Core;
using uiia_adventure.Globals;

namespace uiia_adventure.Systems;

public class DebugSkipSystem : SystemBase
{
    private static KeyboardState _previousState;
    private static double _cooldown = 0.2;
    private static double _timer = 0;

    private readonly SceneFlowController _flowController;

    public DebugSkipSystem(SceneFlowController flowController)
    {
        _flowController = flowController;
    }

    public override void Update(GameTime gameTime, List<GameObject> gameObjects)
    {
        _timer += gameTime.ElapsedGameTime.TotalSeconds;

        var current = Keyboard.GetState();

        if (_timer >= _cooldown && current.IsKeyDown(Keys.F1) && !_previousState.IsKeyDown(Keys.F1))
        {
            Console.WriteLine("F1 pressed, skipping to next scene");
            _flowController.GoToNextScene();
            _timer = 0;
        }

        _previousState = current;
    }
}