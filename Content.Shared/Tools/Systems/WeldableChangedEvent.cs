// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Content.Shared.Tools.Components;

namespace Content.Shared.Tools.Systems;

/// <summary>
///     Raised when <see cref="WeldableComponent"/> has changed.
/// </summary>
[ByRefEvent]
public readonly record struct WeldableChangedEvent(bool IsWelded)
{
    public readonly bool IsWelded = IsWelded;
}