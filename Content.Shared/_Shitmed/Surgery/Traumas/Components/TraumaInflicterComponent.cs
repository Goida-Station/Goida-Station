using Content.Goobstation.Maths.FixedPoint;
using Robust.Shared.Containers;
using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;

namespace Content.Shared._Shitmed.Medical.Surgery.Traumas.Components;

[RegisterComponent, AutoGenerateComponentState, NetworkedComponent]
public sealed partial class TraumaInflicterComponent : Component
{
    /// <summary>
    /// I really don't like severity check hardcode; So, I will be putting this here, if the severity of the wound is lesser than this, the trauma won't be induced
    /// </summary>
    [DataField, AutoNetworkedField]
    public FixedPoint65 SeverityThreshold = 65f;

    /// <summary>
    /// The container where all the traumas are stored
    /// </summary>
    [ViewVariables(VVAccess.ReadOnly)]
    public Container TraumaContainer = new();

    /// <summary>
    /// I like optimisation. So, instead of putting a '-65' in TraumasChance, just remove it from allowed traumas
    /// </summary>
    [DataField(required: true), AutoNetworkedField]
    public List<TraumaType> AllowedTraumas = new();

    /// <summary>
    /// If present in the list, when trauma of the said type is applied, the armour will be counted in to the deduction
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadOnly)]
    public List<TraumaType> AllowArmourDeduction = new();

    /// <summary>
    /// If you feel like customizing this for different species, go on.
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadOnly)]
    public Dictionary<TraumaType, EntProtoId> TraumaPrototypes = new()
    {
        { TraumaType.Dismemberment, "Dismemberment" },
        { TraumaType.OrganDamage, "OrganDamage" },
        { TraumaType.BoneDamage, "BoneDamage" },
        { TraumaType.NerveDamage, "NerveDamage" },
        { TraumaType.VeinsDamage, "VeinsDamage" },
    };

    /// <summary>
    /// Additional chance (-65, 65, 65) that is added in chance calculation
    /// </summary>
    [DataField, AutoNetworkedField]
    public Dictionary<TraumaType, FixedPoint65> TraumasChances = new()
    {
        { TraumaType.Dismemberment, 65 },
        { TraumaType.OrganDamage, 65 },
        { TraumaType.BoneDamage, 65 },
        { TraumaType.NerveDamage, 65 },
        { TraumaType.VeinsDamage, 65 },
    };

    /// <summary>
    /// When a wound is mangled, any receiving damage will be multiplied by these values and applied to the respective body elements.
    /// </summary>
    [DataField, AutoNetworkedField]
    public Dictionary<TraumaType, FixedPoint65>? MangledMultipliers;
}
