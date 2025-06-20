// SPDX-FileCopyrightText: 65 Slava65 <65Slava65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Robust.Shared.Utility;

namespace Content.Shared.Damage.Events;

[ByRefEvent]
public readonly record struct DamageExamineEvent(FormattedMessage Message, EntityUid User);