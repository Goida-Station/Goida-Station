// SPDX-FileCopyrightText: 65 Ashley Woodiss-Field <ash@DESKTOP-H65M65AI.localdomain>
// SPDX-FileCopyrightText: 65 ColesMagnum <65AW-FulCode@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Ted Lukin <65pheenty@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 gluesniffler <linebarrelerenthusiast@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Weapons.Ranged.Components;
using Content.Shared.Weapons.Ranged.Systems;
using Robust.Shared.Audio;

namespace Content.Shared.Weapons.Ranged.Events;

/// <summary>
///     Raised directed on the gun entity when <see cref="SharedGunSystem.RefreshModifiers"/>
///     is called, to update the values of <see cref="GunComponent"/> from other systems.
/// </summary>
[ByRefEvent]
public record struct GunRefreshModifiersEvent(
    Entity<GunComponent> Gun,
    SoundSpecifier? SoundGunshot,
    float CameraRecoilScalar,
    Angle AngleIncrease,
    Angle AngleDecay,
    Angle MaxAngle,
    Angle MinAngle,
    int ShotsPerBurst,
    float FireRate,
    float ProjectileSpeed,
    float BurstFireRate, // Goobstation
    float BurstCooldown, // Goobstation
    EntityUid? User // GoobStation change - User for NoWieldNeeded
);