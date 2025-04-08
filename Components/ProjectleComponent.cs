namespace uiia_adventure.Components;

public class ProjectileComponent : IComponent
{
    public float Lifetime = 3f;
    public float Speed = 300f;
    public float Elapsed = 0f;
    public bool IsFromPlayer = true;

    public bool IsBlocked = false;
}