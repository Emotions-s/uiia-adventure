using Microsoft.Xna.Framework;
using uiia_adventure.Globals;

public abstract class SceneBase
{
    public SceneFlowController FlowController { get; set; }
    public abstract void Load(LevelJsonModel levelData);
    public abstract void Update(GameTime gameTime);
    public abstract void Draw(GameTime gameTime);
}