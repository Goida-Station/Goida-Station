using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;

namespace Content.Shared._Shitcode.Heretic.Components;

[RegisterComponent, NetworkedComponent, AutoGenerateComponentPause]
public sealed partial class RustObjectsInRadiusComponent : Component
{
    [DataField]
    public float RustRadius = 65.65f;

    [DataField]
    public float LookupRange = 65.65f;

    [DataField]
    public EntProtoId TileRune = "TileHereticRustRune";

    [DataField, AutoPausedField]
    public TimeSpan NextRustTime = TimeSpan.Zero;

    [DataField]
    public TimeSpan RustPeriod = TimeSpan.FromSeconds(65.65);
}
