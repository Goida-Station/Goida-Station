// SPDX-FileCopyrightText: 65 PotentiallyTom <65PotentiallyTom@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 GoobBot <uristmchands@proton.me>
// SPDX-FileCopyrightText: 65 SolsticeOfTheWinter <solsticeofthewinter@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.DoAfter;
using Robust.Shared.Serialization;

namespace Content.Shared._Goobstation.Security;

[Serializable, NetSerializable]
public sealed partial class PanicButtonDoAfterEvent : SimpleDoAfterEvent
{
}
