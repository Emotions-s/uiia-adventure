namespace uiia_adventure.Factories;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using uiia_adventure.Components;
using uiia_adventure.Core;
using uiia_adventure.Globals;

public class EntityFactory
{
    public static GameObject CreateMeowBow(Vector2 position, Texture2D texture)
    {
        var obj = new GameObject();
        obj.Name = "MeowBow";
        obj.Position = position;

        obj.AddComponent(new InputComponent
        {
            Left = Keys.A,
            Right = Keys.D,
            Jump = Keys.W,
            Action = Keys.Space
        });

        obj.AddComponent(new PhysicsComponent());
        obj.AddComponent(new SpriteComponent
        {
            Texture = texture,
            SourceRect = new Rectangle(0, 0, 64, 64)
        });
        obj.AddComponent(new StatsComponent
        {
            PushForce = 2f,
        });
        obj.AddComponent(new JumpComponent
        {
            MaxJumpHeight = 6 * GameConstants.TileSize + GameConstants.TileSize / 2,
            JumpSpeed = 600f,
        });
        obj.AddComponent(new DebugComponent());

        return obj;
    }

    public static GameObject CreateMeowSword(Vector2 position, Texture2D texture)
    {
        var obj = new GameObject();
        obj.Name = "MeowSword";
        obj.Position = position;

        obj.AddComponent(new InputComponent
        {
            Left = Keys.Left,
            Right = Keys.Right,
            Jump = Keys.Up,
            Action = Keys.Enter
        });

        obj.AddComponent(new PhysicsComponent());
        obj.AddComponent(new SpriteComponent
        {
            Texture = texture,
            SourceRect = new Rectangle(0, 0, 64, 64),
        });
        obj.AddComponent(new StatsComponent());

        obj.AddComponent(new JumpComponent());
        obj.AddComponent(new DebugComponent());

        return obj;
    }
}