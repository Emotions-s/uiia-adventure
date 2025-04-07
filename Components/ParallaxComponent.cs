namespace uiia_adventure.Components
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    public class ParallaxComponent : IComponent
    {
        public Texture2D Background1 { get; set; }
        public Texture2D Background2 { get; set; }

        public float ParallaxFactor1 { get; set; }
        public float ParallaxFactor2 { get; set; }

        public ParallaxComponent(Texture2D background1, Texture2D background2, float parallaxFactor1 = 0.4f, float parallaxFactor2 = 0.7f)
        {
            Background1 = background1;
            Background2 = background2;
            ParallaxFactor1 = parallaxFactor1;
            ParallaxFactor2 = parallaxFactor2;
        }
    }
}
