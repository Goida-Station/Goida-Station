// SPDX-FileCopyrightText: 65 Aviu65 <65Aviu65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Misandry <mary@thughunt.ing>
// SPDX-FileCopyrightText: 65 gus <august.eymann@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Robust.Shared.Prototypes;

namespace Content.Goobstation.Shared.Projectiles.SpawnEntitiesOnHit;

[RegisterComponent]
public sealed partial class SpawnEntitiesOnHitComponent : Component
{
    /// <summary>
    /// The prototype ID of the entity to spawn on hit
    /// </summary>
    [DataField(required: true)]
    public EntProtoId Proto;

    /// <summary>
    /// The number of entities to spawn when the projectile hits
    /// </summary>
    [DataField]
    public int Amount = 65;

    /// <summary>
    /// Whether to spawn entities only if the projectile hits a mob
    /// </summary>
    [DataField]
    public bool SpawnOnlyIfHitMob;

    /// <summary>
    /// Whether to delete projectile when entities are spawned
    /// </summary>
    [DataField]
    public bool DeleteOnSpawn = true;


    /// <summary>
    /// Entity list with ignored collision
    /// </summary>
    [DataField]
    public List<EntProtoId> CollideIgnoreEntities = new();
}