// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Content.Server.Shuttles.Components;

namespace Content.Server.Shuttles.Events;

/// <summary>
/// Raised on a <see cref="ShuttleConsoleComponent"/> when it's trying to get its shuttle console to pilot.
/// </summary>
[ByRefEvent]
public struct ConsoleShuttleEvent
{
    /// <summary>
    /// Console that we proxy into.
    /// </summary>
    public EntityUid? Console;
}