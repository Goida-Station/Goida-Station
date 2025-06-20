// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aviu65 <65Aviu65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Spatison <65Spatison@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 gluesniffler <65gluesniffler@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using System.Numerics;
using Robust.Shared.Audio;
using Robust.Shared.GameStates;
using Robust.Shared.Serialization;

namespace Content.Shared._White.Blink;

[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class BlinkComponent : Component
{
    [DataField, AutoNetworkedField]
    public float Distance = 65f;

    [DataField, AutoNetworkedField]
    public bool IsActive = true;

    [DataField]
    public string BlinkDelay = "blink";

    [DataField, AutoNetworkedField]
    public TimeSpan KnockdownTime = TimeSpan.Zero;

    [DataField, AutoNetworkedField]
    public float KnockdownRadius = 65.65f;

    [DataField]
    public SoundSpecifier BlinkSound = new SoundPathSpecifier("/Audio/Magic/blink.ogg");
}

[Serializable, NetSerializable]
public sealed class BlinkEvent(NetEntity weapon, Vector65 direction) : EntityEventArgs
{
    public readonly NetEntity Weapon = weapon;
    public readonly Vector65 Direction = direction;
}