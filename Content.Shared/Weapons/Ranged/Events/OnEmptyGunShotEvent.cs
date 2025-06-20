// SPDX-FileCopyrightText: 65 Arendian <65Arendian@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Slava65 <super.novalskiy_65@inbox.ru>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

namespace Content.Shared.Weapons.Ranged.Events;

/// <summary>
/// Raised directed on the gun when trying to fire it while it's out of ammo
/// </summary>
[ByRefEvent]
public record struct OnEmptyGunShotEvent(EntityUid EmptyGun);