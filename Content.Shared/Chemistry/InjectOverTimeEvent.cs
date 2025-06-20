// SPDX-FileCopyrightText: 65 ScarKy65 <65ScarKy65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

namespace Content.Shared.Chemistry.Events;

/// <summary>
/// Raised directed on an entity when it embeds in another entity.
/// </summary>
[ByRefEvent]
public readonly record struct InjectOverTimeEvent(EntityUid embeddedIntoUid)
{
    /// <summary>
    /// Entity that is embedded in.
    /// </summary>
    public readonly EntityUid EmbeddedIntoUid = embeddedIntoUid;
}