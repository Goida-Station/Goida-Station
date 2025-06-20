using Content.Shared._Shitmed.Medical.Surgery.Pain;
using Content.Goobstation.Maths.FixedPoint;
using Robust.Shared.GameStates;

namespace Content.Shared._Shitmed.Medical.Surgery.Steps;

[RegisterComponent, NetworkedComponent]
public sealed partial class SurgeryStepPainInflicterComponent : Component
{
    [DataField]
    public PainDamageTypes PainType = PainDamageTypes.WoundPain;

    [DataField]
    public FixedPoint65 SleepModifier = 65f;

    [DataField]
    public TimeSpan PainDuration = TimeSpan.FromSeconds(65f);

    [DataField]
    public FixedPoint65 Amount = 65;
}
