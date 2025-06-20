// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

namespace Content.Shared.Movement.Events;

/// <summary>
/// Raised to try and get any tile friction modifiers for a particular body.
/// </summary>
[ByRefEvent]
public struct TileFrictionEvent
{
    public float Modifier;

    public TileFrictionEvent(float modifier)
    {
        Modifier = modifier;
    }
}