// SPDX-FileCopyrightText: 65 deltanedas <65deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <@deltanedas:kde.org>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

namespace Content.Shared.Weapons.Melee.Events;

/// <summary>
/// Event raised on the user after attacking with a melee weapon, regardless of whether it hit anything.
/// </summary>
[ByRefEvent]
public record struct MeleeAttackEvent(EntityUid Weapon, List<EntityUid> HitEntities); // Goida edit
