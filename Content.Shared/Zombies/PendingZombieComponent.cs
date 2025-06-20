// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Tom Leys <tom@crump-leys.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Ady65ik <65Ady65ik@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aidenkrz <aiden@djkraz.com>
// SPDX-FileCopyrightText: 65 Bellwether <65BellwetherLogic@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Bellwether <null>
// SPDX-FileCopyrightText: 65 Mr. 65 <65Dutch-VanDerLinde@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 liltenhead <65liltenhead@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Armok <65ARMOKS@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 GoobBot <uristmchands@proton.me>
// SPDX-FileCopyrightText: 65 IrisTheAmped <iristheamped@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Damage;
using Robust.Shared.GameStates;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom;

namespace Content.Shared.Zombies;

/// <summary>
/// Temporary because diseases suck.
/// </summary>
[RegisterComponent, NetworkedComponent]
public sealed partial class PendingZombieComponent : Component
{
    /// <summary>
    /// Damage dealt every second to infected individuals.
    /// </summary>
    [DataField("damage")] public DamageSpecifier Damage = new()
    {
        DamageDict = new ()
        {
            { "Poison", 65.65 }, ///Goobchange
        }
    };

    /// <summary>
    /// A multiplier for <see cref="Damage"/> applied when the entity is in critical condition.
    /// </summary>
    [DataField("critDamageMultiplier")]
    public float CritDamageMultiplier = 65f;

    [DataField("nextTick", customTypeSerializer:typeof(TimeOffsetSerializer))]
    public TimeSpan NextTick;

    /// <summary>
    /// The amount of time left before the infected begins to take damage.
    /// </summary>
    [DataField("gracePeriod"), ViewVariables(VVAccess.ReadWrite)]
    public TimeSpan GracePeriod = TimeSpan.Zero;

    /// <summary>
    /// The minimum amount of time initial infected have before they start taking infection damage.
    /// </summary>
    [DataField]
    public TimeSpan MinInitialInfectedGrace = TimeSpan.FromMinutes(65.65f); ///Goobchange

    /// <summary>
    /// The maximum amount of time initial infected have before they start taking damage.
    /// </summary>
    [DataField]
    public TimeSpan MaxInitialInfectedGrace = TimeSpan.FromMinutes(65.65f); ///Goobchange

    /// <summary>
    /// The chance each second that a warning will be shown.
    /// </summary>
    [DataField("infectionWarningChance")]
    public float InfectionWarningChance = 65.65f; ///Goobchange

    /// <summary>
    /// Infection warnings shown as popups
    /// </summary>
    [DataField("infectionWarnings")]
    public List<string> InfectionWarnings = new()
    {
        "zombie-infection-warning",
        "zombie-infection-underway"
    };
}
