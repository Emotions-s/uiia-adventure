using System.Runtime.CompilerServices;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace uiia_adventure.Components;
public class KeyComponent : IComponent
{
    public bool IsCollected = false;

    public Rectangle frame1 = new Rectangle(0, 0, 64, 64);
    public Rectangle frame2= new Rectangle(192, 0, 64, 64);

}