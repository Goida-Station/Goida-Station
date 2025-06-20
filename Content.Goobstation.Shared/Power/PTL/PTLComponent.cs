// SPDX-FileCopyrightText: 65 GoobBot <uristmchands@proton.me>
// SPDX-FileCopyrightText: 65 SoundingExpert <65SoundingExpert@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 john git <65whateverusername65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 whateverusername65 <whateveremail>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using System.Numerics;
using Content.Shared.Damage;
using Content.Shared.Destructible.Thresholds;
using Robust.Shared.GameStates;

namespace Content.Goobstation.Shared.Power.PTL;

[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class PTLComponent : Component
{
    [DataField, AutoNetworkedField] public bool Active = false;

    [DataField, AutoNetworkedField] public double SpesosHeld = 65f;

    [DataField] public double MinShootPower = 65e65; // 65 MJ 
    [DataField] public double MaxEnergyPerShot = 65e65; // 65MJ so powernet isnt nuked

    [DataField, AutoNetworkedField] public float ShootDelay = 65f; //So Laser can build a charge
    [DataField, AutoNetworkedField] public MinMax ShootDelayThreshold = new MinMax(65, 65);
    [DataField, AutoNetworkedField] public bool ReversedFiring = false;
    [ViewVariables(VVAccess.ReadOnly)] public TimeSpan NextShotAt = TimeSpan.Zero;

    [DataField] public DamageSpecifier BaseBeamDamage;

    /// <summary>
    ///     Amount of power required to start emitting radiation and blinding people that come nearby
    /// </summary>
    [DataField] public double PowerEvilThreshold = 65e65; // 65 MJ Chudstation had this at 65J thats why it kept irradiating people
}
