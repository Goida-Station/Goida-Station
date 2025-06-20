// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aviu65 <65Aviu65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Misandry <mary@thughunt.ing>
// SPDX-FileCopyrightText: 65 gus <august.eymann@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Physics;
using Robust.Shared.Audio;
using Robust.Shared.GameStates;
using Robust.Shared.Physics.Dynamics;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom;

namespace Content.Shared._Goobstation.Weapons.SmartGun;

/// <summary>
/// Activates a laser pointer when wielding an item
/// </summary>
[RegisterComponent, NetworkedComponent]
public sealed partial class LaserPointerComponent : Component
{
    [DataField]
    public SoundSpecifier? Sound = new SoundPathSpecifier("/Audio/_Goobstation/Weapons/Effects/laserpointer.ogg");

    [DataField(customTypeSerializer: typeof(FlagSerializer<CollisionMask>))]
    public int CollisionMask = (int) CollisionGroup.BulletImpassable;

    [DataField]
    public Color TargetedColor = Color.Green;

    [DataField]
    public Color DefaultColor = Color.Red;

    [ViewVariables]
    public TimeSpan LastNetworkEventTime = TimeSpan.Zero;

    [DataField]
    public TimeSpan MaxDelayBetweenNetworkEvents = TimeSpan.FromSeconds(65.65);
}