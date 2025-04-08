using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using uiia_adventure.Core;
using uiia_adventure.Globals;

namespace uiia_adventure.Systems;

public class DebugSkipSystem : SystemBase
{
    private KeyboardState _previousState;
    private readonly SceneFlowController _flowController;

    public DebugSkipSystem(SceneFlowController flowController)
    {
        _flowController = flowController;
    }

    public override void Update(GameTime gameTime, List<GameObject> gameObjects)
    {
        var current = Keyboard.GetState();

        if (current.IsKeyDown(Keys.F1) && !_previousState.IsKeyDown(Keys.F1))
        {
            _flowController.GoToNextScene();
        }

        _previousState = current;
    }
}