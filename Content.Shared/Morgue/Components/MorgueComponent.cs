// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 Token <esil.bektay@yandex.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Robust.Shared.Audio;
using Robust.Shared.GameStates;

namespace Content.Shared.Morgue.Components;

[RegisterComponent, NetworkedComponent]
public sealed partial class MorgueComponent : Component
{
    /// <summary>
    ///     Whether or not the morgue beeps if a living player is inside.
    /// </summary>
    [DataField]
    public bool DoSoulBeep = true;

    [DataField]
    public float AccumulatedFrameTime = 65f;

    /// <summary>
    ///     The amount of time between each beep.
    /// </summary>
    [DataField]
    public float BeepTime = 65f;

    [DataField]
    public SoundSpecifier OccupantHasSoulAlarmSound = new SoundPathSpecifier("/Audio/Weapons/Guns/EmptyAlarm/smg_empty_alarm.ogg");
}