using System.Numerics;
using Robust.Shared.GameStates;

namespace Content.Goida.Useless;

[RegisterComponent, NetworkedComponent]
public sealed partial class PulsatingScaleComponent : Component
{
    [ViewVariables(VVAccess.ReadWrite)] // mf rider
    [DataField]
    public float Intensity { get; set; } = 0.2f;

    [ViewVariables(VVAccess.ReadWrite)]
    [DataField]
    public float CycleTime { get; set; } = 0.5f;

    [ViewVariables(VVAccess.ReadWrite)]
    [DataField]
    public Vector2 BaseScale { get; set; } = Vector2.One;
}
