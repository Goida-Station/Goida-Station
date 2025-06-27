using Robust.Shared.GameStates;

namespace Content.Goida.InvisWatch;

/// <summary>
/// This is used to mark the entity that uses the <see cref="InvisibilityWatchComponent"/>
/// </summary>
[RegisterComponent, NetworkedComponent]
public sealed partial class ActiveInvisibilityWatchComponent : Component
{
    [DataField]
    public EntityUid Watch;
}
