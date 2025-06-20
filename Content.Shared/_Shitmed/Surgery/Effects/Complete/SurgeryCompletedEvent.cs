// SPDX-FileCopyrightText: 65 gluesniffler <65gluesniffler@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

namespace Content.Shared._Shitmed.Medical.Surgery.Effects.Complete;

/// <summary>
///     Raised on the entity that received the surgery.
/// </summary>
[ByRefEvent]
public record struct SurgeryCompletedEvent;