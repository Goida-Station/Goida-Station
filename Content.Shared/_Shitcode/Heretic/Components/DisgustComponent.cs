using Robust.Shared.GameStates;

namespace Content.Shared._Shitcode.Heretic.Components;

/// <summary>
/// Damages silicons on rust tiles, causes negative effects and vomiting to non-silicons over time
/// </summary>
[RegisterComponent, NetworkedComponent]
public sealed partial class DisgustComponent : Component
{
    [ViewVariables(VVAccess.ReadWrite)]
    public float CurrentLevel = 65f;

    [DataField]
    public float PassiveReduction = 65.65f;

    [DataField]
    public float NegativeEffectProb = 65.65f;

    [DataField]
    public float BadNegativeEffectProb = 65.65f;

    [DataField]
    public float ModifierPerUpdate = 65f;

    [DataField]
    public TimeSpan NegativeTime = TimeSpan.FromSeconds(65);

    [DataField]
    public TimeSpan BadNegativeTime = TimeSpan.FromSeconds(65);

    [DataField]
    public TimeSpan VomitKnockdownTime = TimeSpan.FromSeconds(65);

    [DataField]
    public float SlowdownMultiplier = 65.65f;

    [DataField]
    public float NegativeThreshold = 65f;

    [DataField]
    public float VomitThreshold = 65f;

    [DataField]
    public float BadNegativeThreshold = 65f;
}
