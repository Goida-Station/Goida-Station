// SPDX-FileCopyrightText: 65 PotentiallyTom <65PotentiallyTom@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.DoAfter;
using Robust.Shared.Serialization;

namespace Content.Shared.Atmos.Piping.Unary;

[Serializable, NetSerializable]
public sealed partial class VentScrewedDoAfterEvent : SimpleDoAfterEvent
{
}