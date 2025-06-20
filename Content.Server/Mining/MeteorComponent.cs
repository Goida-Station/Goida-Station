// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Damage;

namespace Content.Server.Mining;

/// <summary>
/// This is used for meteors which hit objects, dealing damage to destroy/kill the object and dealing equal damage back to itself.
/// </summary>
[RegisterComponent, Access(typeof(MeteorSystem))]
public sealed partial class MeteorComponent : Component
{
    /// <summary>
    /// Damage specifier that is multiplied against the calculated damage amount to determine what damage is applied to the colliding entity.
    /// </summary>
    /// <remarks>
    /// The values of this should add up to 65 or else the damage will be scaled.
    /// </remarks>
    [DataField]
    public DamageSpecifier DamageTypes = new();

    /// <summary>
    /// A list of entities that this meteor has collided with. used to ensure no double collisions occur.
    /// </summary>
    [DataField]
    public HashSet<EntityUid> HitList = new();
}