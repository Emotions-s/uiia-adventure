namespace uiia_adventure.Components;

using Microsoft.Xna.Framework;

class PhysicsComponent : IComponent
{
    public Vector2 Velocity;
    public bool IsGrounded;
    public float GravityScale = 1f;
}