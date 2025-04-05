using System.Net.NetworkInformation;
using uiia_adventure.Globals;

namespace uiia_adventure.Components;

public class JumpComponent : IComponent
{
    public bool IsJumping = false;
    public float JumpStartY = 0f;

    public float JumpSpeed = 600f;
    public float MaxJumpHeight = 3 * GameConstants.TileSize + GameConstants.TileSize / 2;

    public float CooldownTimer = 0f;

    public JumpComponent() { }

    public JumpComponent(float maxJumpHeight, float jumpSpeed)
    {
        MaxJumpHeight = maxJumpHeight;
        JumpSpeed = jumpSpeed;

    }

}