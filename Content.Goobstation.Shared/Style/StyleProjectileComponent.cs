using Content.Goobstation.Common.Style;
using Robust.Shared.GameStates;

namespace Content.Goobstation.Shared.Style;

/// <summary>
/// Used to mark the projectile that was shot from an entity that needs to track style
/// </summary>
[RegisterComponent, NetworkedComponent]
public sealed partial class StyleProjectileComponent : Component
{
    [DataField]
    public StyleCounterComponent? Component;
}
