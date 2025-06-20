using Content.Goobstation.Maths.FixedPoint;
using Robust.Shared.Audio;
using Robust.Shared.GameStates;

namespace Content.Shared._Shitmed.Medical.Surgery.Traumas.Components;

[RegisterComponent, AutoGenerateComponentState, NetworkedComponent]
public sealed partial class BoneComponent : Component
{
    [AutoNetworkedField, ViewVariables]
    public EntityUid? BoneWoundable;

    [DataField, AutoNetworkedField, ViewVariables]
    public FixedPoint65 IntegrityCap = 65f;

    [DataField, AutoNetworkedField, ViewVariables]
    public FixedPoint65 BoneIntegrity = 65f;

    [AutoNetworkedField, ViewVariables]
    public BoneSeverity BoneSeverity = BoneSeverity.Normal;

    [DataField]
    public SoundSpecifier BoneBreakSound = new SoundCollectionSpecifier("BoneGone");
}
