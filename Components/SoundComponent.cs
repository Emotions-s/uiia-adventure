using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

namespace uiia_adventure.Components
{
    public class SoundComponent : IComponent
    {
        public SoundEffect? JumpSound;
        public Song? BackgroundMusic;
        public float Volume = 1.0f;
        public bool PlayMusicOnStart = true;
        public bool PlayJumpSound = false;
    }
}