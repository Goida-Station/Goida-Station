// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Robust.Shared.Serialization;

namespace Content.Shared.Pointing;

// TODO just make pointing properly predicted?
// So true
/// <summary>
///     Event raised when someone runs the client-side pointing verb.
/// </summary>
[Serializable, NetSerializable]
public sealed class PointingAttemptEvent : EntityEventArgs
{
    public NetEntity Target;

    public PointingAttemptEvent(NetEntity target)
    {
        Target = target;
    }
}

/// <summary>
/// Raised on the entity who is pointing after they point at something.
/// </summary>
/// <param name="Pointed"></param>
[ByRefEvent]
public readonly record struct AfterPointedAtEvent(EntityUid Pointed);

/// <summary>
/// Raised on an entity after they are pointed at by another entity.
/// </summary>
/// <param name="Pointer"></param>
[ByRefEvent]
public readonly record struct AfterGotPointedAtEvent(EntityUid Pointer);