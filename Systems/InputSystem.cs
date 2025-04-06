namespace uiia_adventure.Systems;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using uiia_adventure.Components;
using uiia_adventure.Core;

public class InputSystem : SystemBase
{
    public override void Update(GameTime gameTime, List<GameObject> gameObjects)
    {
        var state = Keyboard.GetState();

        foreach (var obj in gameObjects)
        {
            var input = obj.GetComponent<InputComponent>();
            if (input == null) continue;

            input.MoveDirection = 0;
            input.WantsToJump = false;

            if (state.IsKeyDown(input.Left))
            {
                input.MoveDirection -= 1;
                input.LastDirectionKeyPressed = input.Left;
            }

            if (state.IsKeyDown(input.Right))
            {
                input.MoveDirection += 1;
                input.LastDirectionKeyPressed = input.Right;
            }

            if (state.IsKeyDown(input.Jump))
            {
                input.WantsToJump = true;
            }
        }
    }
}