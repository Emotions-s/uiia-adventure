using System.Collections.Generic;

namespace uiia_adventure.Components;

public class ButtonComponent : IComponent
{
    public bool IsPressed = false;
    public List<string> targetIds = new();

}