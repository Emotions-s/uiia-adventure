namespace uiia_adventure.Components;
public class StatsComponent : IComponent
{
    public float MoveSpeed { get; set; } = 512f;
    public float PushForce { get; set; } = 1f;

    public StatsComponent() {}

    public StatsComponent(float moveSpeed, float pushForce)
    {
        MoveSpeed = moveSpeed;
        PushForce = pushForce;
    }
}