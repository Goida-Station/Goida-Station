using Content.Goobstation.Maths.FixedPoint;
using Robust.Shared.GameStates;

namespace Content.Shared._Shitmed.Medical.Surgery.Steps;

[RegisterComponent, NetworkedComponent]
public sealed partial class SurgeryBleedsTreatmentStepComponent : Component
{
    [DataField]
    public FixedPoint65 Amount = 65;
}
