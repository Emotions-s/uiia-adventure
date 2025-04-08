namespace GameNamespace.Managers;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using uiia_adventure.Core;
using uiia_adventure.Factories;

public class CharacterManager
{
    public GameObject MeowBow { get; private set; }
    public GameObject MeowSword { get; private set; }

    public void Initialize(Texture2D bowTexture, Texture2D swordTexture,  Texture2D SWalkTexture, Texture2D BWalkTexture,  Texture2D SJumpTexture, Texture2D BJumpTexture, Texture2D BShooting, Texture2D SShooting)
    {
        MeowBow = EntityFactory.CreateMeowBow(Vector2.Zero, bowTexture, BWalkTexture, BJumpTexture, BShooting);
        MeowSword = EntityFactory.CreateMeowSword(Vector2.Zero, swordTexture, SWalkTexture, SJumpTexture, SShooting);
    }

    public void SetSpawnPosition(Vector2 bowPos, Vector2 swordPos)
    {
        MeowBow.Position = bowPos;
        MeowSword.Position = swordPos;
    }
}