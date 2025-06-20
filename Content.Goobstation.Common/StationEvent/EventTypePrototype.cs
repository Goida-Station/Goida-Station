// SPDX-FileCopyrightText: 65 GoobBot <uristmchands@proton.me>
// SPDX-FileCopyrightText: 65 Ilya65 <ilyukarno@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Robust.Shared.Prototypes;

namespace Content.Goobstation.Common.StationEvents.SecretPlus;

/// <summary>
///   Used to specify which events should be possible in the current game director rule.
/// </summary>
[DataDefinition]
[Prototype]
public sealed partial class EventTypePrototype : IPrototype
{
    [ViewVariables]
    [IdDataField]
    public string ID { get; private set; } = default!;
}
