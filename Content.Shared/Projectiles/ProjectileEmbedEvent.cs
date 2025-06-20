// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Dakamakat <65dakamakat@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <comedian_vs_clown@hotmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

namespace Content.Shared.Projectiles;

/// <summary>
/// Raised directed on an entity when it embeds into something.
/// </summary>
[ByRefEvent]
public readonly record struct ProjectileEmbedEvent(EntityUid? Shooter, EntityUid Weapon, EntityUid Embedded);