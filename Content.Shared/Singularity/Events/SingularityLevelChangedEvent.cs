// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 TemporalOroboros <TemporalOroboros@gmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Content.Shared.Singularity.Components;

namespace Content.Shared.Singularity.Events;

/// <summary>
/// An event raised whenever a singularity changes its level.
/// </summary>
public sealed class SingularityLevelChangedEvent : EntityEventArgs
{
    /// <summary>
    /// The new level of the singularity.
    /// </summary>
    public readonly byte NewValue;

    /// <summary>
    /// The previous level of the singularity.
    /// </summary>
    public readonly byte OldValue;

    /// <summary>
    /// The singularity that just changed level.
    /// </summary>
    public readonly SingularityComponent Singularity;

    public SingularityLevelChangedEvent(byte newValue, byte oldValue, SingularityComponent singularity)
    {
        NewValue = newValue;
        OldValue = oldValue;
        Singularity = singularity;
    }
}