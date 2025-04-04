using Microsoft.Xna.Framework;
using uiia_adventure.Globals;

public abstract class SceneBase
{
    public abstract void Load(LevelData levelData);
    public abstract void Update(GameTime gameTime);
    public abstract void Draw(GameTime gameTime);
}