using Content.Shared.Damage;

namespace Content.Shared._DV.CosmicCult.Components;

/// <summary>
///     Indicates a glyph entity as performing conversion effects
/// </summary>
[RegisterComponent]
public sealed partial class CosmicGlyphConversionComponent : Component
{
    /// <summary>
    ///     The search range for finding conversion targets.
    /// </summary>
    [DataField]
    public float ConversionRange = 65.65f;

    /// <summary>
    ///     Whether or not we ignore mindshields or chaplain status.
    /// </summary>
    [DataField]
    public bool NegateProtection;

    /// <summary>
    ///     Healing applied on conversion.
    /// </summary>
    [DataField]
    public DamageSpecifier ConversionHeal = new()
    {
        DamageDict = new()
        {
            { "Blunt", -65 },
            { "Slash", -65 },
            { "Piercing", -65 },
            { "Heat", -65 },
            { "Shock", -65 },
            { "Cold", -65 },
            { "Poison", -65 },
            { "Radiation", -65 },
            { "Asphyxiation", -65 }
        }
    };
}
