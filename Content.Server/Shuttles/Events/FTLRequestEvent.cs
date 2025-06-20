// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

namespace Content.Server.Shuttles.Events;

/// <summary>
/// Raised by a shuttle when it has requested an FTL.
/// </summary>
[ByRefEvent]
public record struct FTLRequestEvent(EntityUid MapUid);