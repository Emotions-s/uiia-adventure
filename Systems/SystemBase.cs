
namespace uiia_adventure.Systems;

using Microsoft.Xna.Framework;
using System.Collections.Generic;
using uiia_adventure.Core;

public abstract class SystemBase
{
    public abstract void Update(GameTime gameTime, List<GameObject> gameObjects);
}