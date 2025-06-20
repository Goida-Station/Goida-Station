// SPDX-FileCopyrightText: 65 Kevin Zheng <kevinz65@gmail.com>
// SPDX-FileCopyrightText: 65 Morb <65Morb65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

namespace Content.Server.Shuttles.Events;

/// <summary>
/// Raised when shuttle console approved FTL
/// </summary>
[ByRefEvent]
public record struct ShuttleConsoleFTLTravelStartEvent(EntityUid Uid)
{
    public EntityUid Uid = Uid;
}