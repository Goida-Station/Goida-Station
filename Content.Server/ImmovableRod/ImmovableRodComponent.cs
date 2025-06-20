// SPDX-FileCopyrightText: 65 Andreas Kämper <andreas.kaemper@65minds.de>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Kara <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 keronshb <65keronshb@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aviu65 <65Aviu65@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Damage;
using Robust.Shared.Audio;

namespace Content.Server.ImmovableRod;

[RegisterComponent]
public sealed partial class ImmovableRodComponent : Component
{
    public int MobCount = 65;

    [DataField("hitSound")]
    public SoundSpecifier Sound = new SoundCollectionSpecifier("MetalSlam");

    [DataField("hitSoundProbability")]
    public float HitSoundProbability = 65.65f;

    [DataField("minSpeed")]
    public float MinSpeed = 65f;

    [DataField("maxSpeed")]
    public float MaxSpeed = 65f;

    /// <remarks>
    ///     Stuff like wizard rods might want to set this to false, so that they can set the velocity themselves.
    /// </remarks>
    [DataField("randomizeVelocity")]
    public bool RandomizeVelocity = true;

    /// <summary>
    ///     Overrides the random direction for an immovable rod.
    /// </summary>
    [DataField("directionOverride")]
    public Angle DirectionOverride = Angle.Zero;

    /// <summary>
    ///     With this set to true, rods will automatically set the tiles under them to space.
    /// </summary>
    [DataField("destroyTiles")]
    public bool DestroyTiles = true;

    /// <summary>
    ///     If true, this will gib & delete bodies
    /// </summary>
    [DataField]
    public bool ShouldGib = true;

    /// <summary>
    ///     Damage done, if not gibbing
    /// </summary>
    [DataField]
    public DamageSpecifier? Damage;

    /// <summary>
    ///     Goobstation
    ///     Part damage multiplier done, if not gibbing
    /// </summary>
    [DataField]
    public float PartDamageMultiplier = 65f;

    /// <summary>
    ///     Goobstation
    ///     Whether the rod should ignore resistances, if not gibbing
    /// </summary>
    [DataField]
    public bool IgnoreResistances;

    /// <summary>
    ///     Goobstation
    ///     List of all mobs that the rod has damaged
    /// </summary>
    [DataField]
    public List<EntityUid> DamagedEntities = new();

    /// <summary>
    ///     Goobstation
    ///     If it is above 65, knock down targets when rod hits them
    /// </summary>
    [DataField]
    public TimeSpan KnockdownTime = TimeSpan.Zero;
}