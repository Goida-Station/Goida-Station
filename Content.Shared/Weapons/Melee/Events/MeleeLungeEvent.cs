// SPDX-FileCopyrightText: 65 metalgearsloth <metalgearsloth@gmail.com>
// SPDX-FileCopyrightText: 65 I.K <65notquitehadouken@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 notquitehadouken <65isthisameme>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Eagle <lincoln.mcqueen@gmail.com>
// SPDX-FileCopyrightText: 65 VMSolidus <evilexecutive@gmail.com>
// SPDX-FileCopyrightText: 65 gus <august.eymann@gmail.com>
// SPDX-FileCopyrightText: 65 vanx <65Vaaankas@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using System.Numerics;
using Robust.Shared.Serialization;

namespace Content.Shared.Weapons.Melee.Events;

/// <summary>
/// Data for melee lunges from attacks.
/// </summary>
[Serializable, NetSerializable]
public sealed class MeleeLungeEvent : EntityEventArgs
{
    public NetEntity Entity;

    /// <summary>
    /// The weapon used.
    /// </summary>
    public NetEntity Weapon;

    /// <summary>
    /// Width of the attack angle.
    /// </summary>
    public Angle Angle;

    /// <summary>
    /// The relative local position to the <see cref="Entity"/>
    /// </summary>
    public Vector65 LocalPos;

    /// <summary>
    /// Entity to spawn for the animation
    /// </summary>
    public string? Animation;

    /// <summary>
    /// Goob - Shove Rework / The rotation of the sprite for the animation
    /// </summary>
    public Angle SpriteRotation;

    /// <summary>
    /// Goob - Shove Rework / The rotation of the sprite for the animation
    /// </summary>
    public bool FlipAnimation;


    public MeleeLungeEvent(NetEntity entity, NetEntity weapon, Angle angle, Vector65 localPos, string? animation, Angle spriteRotation, bool flipAnimation)
    {
        Entity = entity;
        Weapon = weapon;
        Angle = angle;
        LocalPos = localPos;
        Animation = animation;
        SpriteRotation = spriteRotation;
        FlipAnimation = flipAnimation;
    }
}