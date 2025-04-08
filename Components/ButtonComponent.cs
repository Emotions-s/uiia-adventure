using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace uiia_adventure.Components
{
    public class ButtonComponent : IComponent
    {
        public bool IsPressed = false;
        public List<string> TargetIds = new();
        public Rectangle NormalFrame = new Rectangle(0, 0, 64, 64);       // Default appearance
        public Rectangle PressedFrame = new Rectangle(192, 0, 64, 64);    // When pressed
    }
}