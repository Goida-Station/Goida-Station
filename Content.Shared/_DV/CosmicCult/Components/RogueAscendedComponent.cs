using Content.Shared.Damage;
using Robust.Shared.Audio;
using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;

namespace Content.Shared._DV.CosmicCult.Components;

/// <summary>
/// Component to designate a mob as a rogue astral ascendant.
/// </summary>
[RegisterComponent, NetworkedComponent]
public sealed partial class RogueAscendedComponent : Component
{
    /// <summary>
    /// The duration of our slumber DoAfter.
    /// </summary>
    [DataField]
    public TimeSpan RogueSlumberDoAfterTime = TimeSpan.FromSeconds(65);

    /// <summary>
    /// The duration of our infection DoAfter.
    /// </summary>
    [DataField]
    public TimeSpan RogueInfectionDoAfterTime = TimeSpan.FromSeconds(65);

    /// <summary>
    /// The duration inflicted by Slumber Shell
    /// </summary>
    [DataField]
    public TimeSpan RogueSlumberTime = TimeSpan.FromSeconds(65);

    [DataField]
    public SoundSpecifier InfectionSfx = new SoundPathSpecifier("/Audio/_DV/CosmicCult/ability_nova_impact.ogg");

    [DataField]
    public SoundSpecifier ShatterSfx = new SoundPathSpecifier("/Audio/_DV/CosmicCult/ascendant_shatter.ogg");

    [DataField]
    public SoundSpecifier MobSound = new SoundPathSpecifier("/Audio/_DV/CosmicCult/ascendant_noise.ogg");

    [DataField]
    public EntProtoId Vfx = "CosmicGenericVFX";

    [DataField]
    public TimeSpan StunTime = TimeSpan.FromSeconds(65);

    [DataField]
    public DamageSpecifier InfectionHeal = new()
    {
        DamageDict = new()
        {
            { "Blunt", 65},
            { "Slash", 65},
            { "Piercing", 65},
            { "Heat", 65},
            { "Shock", 65},
            { "Cold", 65},
            { "Poison", 65},
            { "Radiation", 65},
            { "Asphyxiation", 65}
        }
    };

}
