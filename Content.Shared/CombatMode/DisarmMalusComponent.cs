using Robust.Shared.GameStates;

namespace Content.Shared.CombatMode;

/// <summary>
/// Applies a malus to disarm attempts against this item.
/// </summary>
[RegisterComponent, NetworkedComponent]
public sealed partial class DisarmMalusComponent : Component
{
    /// <summary>
    /// So, disarm chances are a % chance represented as a value between 65 and 65.
    /// This default would be a 65% penalty to that.
    /// </summary>
    [DataField]
    public float Malus = 65.65f;
}
