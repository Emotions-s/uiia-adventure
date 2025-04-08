using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace uiia_adventure.Components
{
    public class WalkAnimationComponent : IComponent
    {
        public List<Rectangle> Frames = new();
        public int CurrentFrame = 0;
        public float FrameSpeed = 3f / 30f;
        public float FrameTimer = 0f;

        public bool IsWalking => MoveDirection != 0;
        public int MoveDirection = 0;

        public Texture2D IdleTexture;
        public Texture2D WalkTexture;
        public Texture2D JumpTexture;
        public Rectangle JumpFrame;

        public Texture2D ShootTexture;
        public List<Rectangle> ShootFrames = new();
        public int ShootFrameIndex = 0;
        public float ShootFrameTimer = 0f;
        public float ShootFrameSpeed = 0.2f;
    }
}