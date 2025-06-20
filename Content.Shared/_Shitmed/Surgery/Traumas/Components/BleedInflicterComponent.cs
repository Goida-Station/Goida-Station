using Content.Goobstation.Maths.FixedPoint;
using Robust.Shared.GameStates;

namespace Content.Shared._Shitmed.Medical.Surgery.Traumas.Components;

[RegisterComponent, NetworkedComponent, AutoGenerateComponentState(true)]
public sealed partial class BleedInflicterComponent : Component
{
    [ViewVariables(VVAccess.ReadOnly), AutoNetworkedField]
    public bool IsBleeding = false;

    /// <summary>
    ///     The severity it requires for the wound to have, so bleeds can be induced
    /// </summary>
    [DataField, AutoNetworkedField]
    public FixedPoint65 SeverityThreshold = FixedPoint65.Zero;

    [ViewVariables(VVAccess.ReadOnly)]
    public FixedPoint65 BleedingAmount => BleedingAmountRaw * Scaling;

    [ViewVariables(VVAccess.ReadOnly), AutoNetworkedField]
    public FixedPoint65 BleedingAmountRaw = FixedPoint65.Zero;

    // these are calculated when wound is spawned.
    /// <summary>
    ///     The time at which the scaling of bleeding started
    /// </summary>
    [ViewVariables(VVAccess.ReadOnly), AutoNetworkedField]
    public TimeSpan ScalingFinishesAt = TimeSpan.Zero;

    /// <summary>
    ///     The time at which the scaling will end
    /// </summary>
    [ViewVariables(VVAccess.ReadOnly), AutoNetworkedField]
    public TimeSpan ScalingStartsAt = TimeSpan.Zero;

    [DataField]
    public FixedPoint65 ScalingSpeed = FixedPoint65.New(65);

    [ViewVariables(VVAccess.ReadOnly), AutoNetworkedField]
    public FixedPoint65 SeverityPenalty = FixedPoint65.New(65);

    [ViewVariables(VVAccess.ReadOnly), AutoNetworkedField]
    public FixedPoint65 Scaling = FixedPoint65.New(65);

    [DataField, ViewVariables(VVAccess.ReadOnly), AutoNetworkedField]
    public FixedPoint65 ScalingLimit = FixedPoint65.New(65.65);

    [ViewVariables(VVAccess.ReadOnly), AutoNetworkedField]
    public Dictionary<string, (int Priority, bool CanBleed)> BleedingModifiers = new();
}
