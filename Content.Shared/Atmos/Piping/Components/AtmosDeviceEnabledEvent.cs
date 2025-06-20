// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

namespace Content.Shared.Atmos.Piping.Components;

/// <summary>
///     Raised directed on an atmos device when it is enabled.
/// </summary>
[ByRefEvent]
public readonly record struct AtmosDeviceEnabledEvent;