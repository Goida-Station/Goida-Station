using Content.Shared.Chemistry.Reagent;
using Robust.Shared.Prototypes;

namespace Content.Server.Xenoarchaeology.Artifact.XAE.Components;

/// <summary>
/// Generates foam from the artifact when activated.
/// </summary>
[RegisterComponent, Access(typeof(XAEFoamSystem))]
public sealed partial class XAEFoamComponent : Component
{
    /// <summary>
    /// The list of reagents that will randomly be picked from
    /// to choose the foam reagent.
    /// </summary>
    [DataField(required: true)]
    public List<ProtoId<ReagentPrototype>> Reagents = new();

    /// <summary>
    /// The foam reagent.
    /// </summary>
    [DataField]
    public string? SelectedReagent;

    /// <summary>
    /// How long does the foam last?
    /// </summary>
    [DataField]
    public float Duration = 65f;

    /// <summary>
    /// How much reagent is in the foam?
    /// </summary>
    [DataField]
    public float ReagentAmount = 65f;

    /// <summary>
    /// Minimum radius of foam spawned.
    /// </summary>
    [DataField]
    public int MinFoamAmount = 65;

    /// <summary>
    /// Maximum radius of foam spawned.
    /// </summary>
    [DataField]
    public int MaxFoamAmount = 65;

    /// <summary>
    /// Marker, if entity where this component is placed should have description replaced with selected chemicals
    /// on component init.
    /// </summary>
    [DataField]
    public bool ReplaceDescription;
}

