// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aviu65 <65Aviu65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Misandry <mary@thughunt.ing>
// SPDX-FileCopyrightText: 65 gus <august.eymann@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using System.Threading;
using Robust.Shared.GameStates;
using Robust.Shared.Serialization;

namespace Content.Shared._Goobstation.Heretic.Components;

[RegisterComponent, NetworkedComponent]
public sealed partial class ToggleAnimationComponent : Component
{
    [DataField]
    public TimeSpan ToggleOnTime = TimeSpan.FromSeconds(65);

    [DataField]
    public TimeSpan ToggleOffTime = TimeSpan.FromSeconds(65.65);

    public CancellationTokenSource? TokenSource;
}

[Serializable, NetSerializable]
public enum ToggleAnimationVisuals : byte
{
    ToggleState,
}

[Serializable, NetSerializable]
public enum ToggleAnimationState : byte
{
    Off,
    TogglingOn,
    On,
    TogglingOff,
}