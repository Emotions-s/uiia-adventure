namespace uiia_adventure.Factories;

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using uiia_adventure.Components;
using uiia_adventure.Core;
using uiia_adventure.Globals;
using System.Collections.Generic;


public class EntityFactory
{
    public static GameObject CreateMeowBow(Vector2 position, Texture2D texture, Texture2D walkTexture, Texture2D jumpTexture)
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
            JumpSpeed = 800f,
        });
        obj.AddComponent(new DebugComponent());
        obj.AddComponent(new WalkAnimationComponent
        {
            IdleTexture = texture,
            WalkTexture = walkTexture,
            JumpTexture = jumpTexture,
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
        });

        obj.AddComponent(new SoundComponent
        {
            JumpSound = Game1.JumpSound
        });

        return obj;
    }

    public static GameObject CreateMeowSword(Vector2 position, Texture2D texture, Texture2D walkTexture, Texture2D jumpTexture)
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
        obj.AddComponent(new WalkAnimationComponent
        {
            IdleTexture = texture,
            WalkTexture = walkTexture,
            JumpTexture = jumpTexture,
            
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
        });

        obj.AddComponent(new SoundComponent
        {
            JumpSound = Game1.JumpSound
        });

        return obj;
    }
}