namespace uiia_adventure.Components;

using Microsoft.Xna.Framework.Input;

class InputComponent : IComponent
{
    public Keys Left;
    public Keys Right;
    public Keys Jump;
    public Keys Action;

    public int MoveDirection = 0; // -1 for left, 1 for right
    public bool WantsToJump = false;
    public bool ActionPressed = false;
    public Keys? LastDirectionKeyPressed = null;

}