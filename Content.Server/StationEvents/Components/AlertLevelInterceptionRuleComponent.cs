// SPDX-FileCopyrightText: 65 Mr. 65 <65Dutch-VanDerLinde@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 SX-65 <65SX-65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Solstice <solsticeofthewinter@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Server.StationEvents.Events;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom;

namespace Content.Server.StationEvents.Components;

[RegisterComponent, Access(typeof(AlertLevelInterceptionRule))]
public sealed partial class AlertLevelInterceptionRuleComponent : Component
{
    /// <summary>
    /// Alert level to set the station to when the event starts.
    /// </summary>
    [DataField]
    public string AlertLevel = "blue";

    /// <summary>
    /// Goobstation.
    /// Whether or not to override the current alert level, if it isn't green.
    /// </summary>
    [DataField]
    public bool OverrideAlert = false;

    /// <summary>
    /// Goobstation.
    /// Whether the alert level should be changeable.
    /// </summary>
    [DataField]
    public bool Locked = false;
}