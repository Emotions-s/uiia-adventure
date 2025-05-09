namespace uiia_adventure.Factories;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using uiia_adventure.Components;
using uiia_adventure.Core;
using uiia_adventure.Globals;
using System.Collections.Generic;

public class EntityFactory
{
    public static GameObject CreateMeowBow(Vector2 position, Texture2D texture, Texture2D walkTexture, Texture2D jumpTexture, Texture2D shootTexture)
    {
        var obj = new GameObject();
        obj.Name = GameConstants.MeowBowName;
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
            SourceRect = new Rectangle(0, 0, 64, 64)
        });
        obj.AddComponent(new StatsComponent());
        obj.AddComponent(new JumpComponent
        {
            MaxJumpHeight = 6 * GameConstants.TileSize + GameConstants.TileSize / 2,
            JumpSpeed = 800f,
        });
        obj.AddComponent(new DebugComponent());
        obj.AddComponent(new WalkAnimationComponent
        {
            IdleTexture = texture,
            WalkTexture = walkTexture,
            JumpTexture = jumpTexture,
            ShootTexture = shootTexture,
            Frames = new List<Rectangle>
            {
                new Rectangle(0, 0, 64, 64),
                new Rectangle(0, 64, 64, 64),
                new Rectangle(0, 128, 64, 64),
                new Rectangle(0, 192, 64, 64),
                new Rectangle(0, 256, 64, 64),
                new Rectangle(0, 320, 64, 64),
                new Rectangle(0, 384, 64, 64),
                new Rectangle(0, 448, 64, 64),
            },
            JumpFrame = new Rectangle(0, 0, 64, 64),
            ShootFrames = new List<Rectangle>
            {
                new Rectangle(0, 0, 64, 64),
                new Rectangle(0, 64, 64, 64),
                new Rectangle(0, 128, 64, 64),
            },
        });


        obj.AddComponent(new SoundComponent
        {
            JumpSound = Game1.JumpSound1
        });
        obj.AddComponent(new CanShootComponent());

        return obj;
    }

    public static GameObject CreateMeowSword(Vector2 position, Texture2D texture, Texture2D walkTexture, Texture2D jumpTexture, Texture2D shootTexture)
    {
        var obj = new GameObject();
        obj.Name = GameConstants.MeowSwordName;
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
            SourceRect = new Rectangle(0, 0, 64, 64),
        });
        obj.AddComponent(new StatsComponent());

        obj.AddComponent(new JumpComponent());
        obj.AddComponent(new DebugComponent());
        obj.AddComponent(new WalkAnimationComponent
        {
            IdleTexture = texture,
            WalkTexture = walkTexture,
            JumpTexture = jumpTexture,
            ShootTexture = shootTexture,

            Frames = new List<Rectangle>
            {
                new Rectangle(0, 0, 64, 64),
                new Rectangle(0, 64, 64, 64),
                new Rectangle(0, 128, 64, 64),
                new Rectangle(0, 192, 64, 64),
                new Rectangle(0, 256, 64, 64),
                new Rectangle(0, 320, 64, 64),
                new Rectangle(0, 384, 64, 64),
                new Rectangle(0, 448, 64, 64),
            },
            JumpFrame = new Rectangle(0, 0, 64, 64),
            ShootFrames = new List<Rectangle>
            {
                new Rectangle(0, 0, 64, 64),
                new Rectangle(0, 64, 64, 64),
                new Rectangle(0, 128, 64, 64),
            },
        });

        obj.AddComponent(new CanPushComponent());

        obj.AddComponent(new SoundComponent
        {
            JumpSound = Game1.JumpSound2,
            Volume = 0.3f,
        });

        obj.AddComponent(new MeleeComponent());

        return obj;
    }
}