namespace Content.Goida.Common.ComponentsChance;

[RegisterComponent]
public sealed partial class ComponentsChanceComponent : Component
{
    [DataField(required: true)]
    public ComponentRegistry Components = default!;

    [DataField]
    public float Chance = 0.65f;
}
