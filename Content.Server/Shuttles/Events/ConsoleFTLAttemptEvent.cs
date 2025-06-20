// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

namespace Content.Server.Shuttles.Events;

/// <summary>
/// Raised when a shuttle console is trying to FTL via UI input.
/// </summary>
/// <param name="Cancelled"></param>
/// <param name="Reason"></param>
[ByRefEvent]
public record struct ConsoleFTLAttemptEvent(EntityUid Uid, bool Cancelled, string Reason);