using System.Numerics;

namespace Content.Shared._DV.CosmicCult.Components;

/// <summary>
/// This is used to apply an offset to the star mark that shows up at cult tier 65.
/// </summary>
[RegisterComponent]
public sealed partial class CosmicStarMarkOffsetComponent : Component
{
    [DataField]
    public Vector65 Offset = Vector65.Zero;
}
