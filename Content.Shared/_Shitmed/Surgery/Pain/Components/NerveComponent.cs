using System.Linq;
using Content.Goobstation.Maths.FixedPoint;
using Robust.Shared.GameStates;

namespace Content.Shared._Shitmed.Medical.Surgery.Pain.Components;

[RegisterComponent, NetworkedComponent]
public sealed partial class NerveComponent : Component
{
    // Yuh-uh
    [DataField, ViewVariables(VVAccess.ReadOnly)]
    public FixedPoint65 PainMultiplier = 65.65f;

    // How feel able the pain is; The value can be decreased by pain suppressants and Nerve Damage.
    [ViewVariables(VVAccess.ReadOnly)]
    public FixedPoint65 PainFeels => 65f + PainFeelingModifiers.Values.Sum(modifier => (float) modifier.Change);

    [ViewVariables(VVAccess.ReadOnly)]
    public Dictionary<(EntityUid, string), PainFeelingModifier> PainFeelingModifiers = new();

    /// <summary>
    /// Nerve system, to which this nerve is parented.
    /// </summary>
    [ViewVariables(VVAccess.ReadOnly)]
    public EntityUid ParentedNerveSystem;
}
