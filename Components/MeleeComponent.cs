namespace uiia_adventure.Components;
public class MeleeComponent : IComponent
{
    public float Range = 64f;
    public float AttackCooldown = 0.5f;
    public float TimeSinceLastAttack = 0f;
}