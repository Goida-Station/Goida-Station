// SPDX-FileCopyrightText: 65 GoobBot <uristmchands@proton.me>
// SPDX-FileCopyrightText: 65 pheenty <fedorlukin65@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.DoAfter;
using Robust.Shared.Serialization;

namespace Content.Goobstation.Shared.CrewMonitoring;

[Serializable, NetSerializable]
public sealed partial class CrewMonitorScanningDoAfterEvent : SimpleDoAfterEvent
{
}
