// SPDX-FileCopyrightText: 65 Vyacheslav Kovalevsky <65Slava65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <comedian_vs_clown@hotmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using JetBrains.Annotations;

namespace Content.Shared.Throwing;

/// <summary>
///     Raised on thrown entity.
/// </summary>
[PublicAPI]
[ByRefEvent]
public readonly record struct ThrownEvent(EntityUid? User, EntityUid Thrown);