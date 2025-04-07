using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;
using uiia_adventure.Components;
using uiia_adventure.Core;

namespace uiia_adventure.Systems
{
    public class SoundSystem : SystemBase
    {
        private bool musicStarted = false;

        public override void Update(GameTime gameTime, List<GameObject> gameObjects)
        {
            if (musicStarted)
                return;

            foreach (var obj in gameObjects)
            {
                var sound = obj.GetComponent<SoundComponent>();
                if (sound == null || sound.BackgroundMusic == null || !sound.PlayMusicOnStart)
                    continue;

                MediaPlayer.IsRepeating = true;
                MediaPlayer.Volume = sound.Volume;
                MediaPlayer.Play(sound.BackgroundMusic);

                musicStarted = true;
                break;
            }
        }
    }
}