// SPDX-FileCopyrightText: 65 Emisse <65Emisse@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 IProduceWidgets <65IProduceWidgets@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 Plykiya <65Plykiya@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Southbridge <65southbridge-fur@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Spanky <scott@wearejacob.com>
// SPDX-FileCopyrightText: 65 Spessmann <65Spessmann@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Ubaser <65UbaserB@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Misandry <mary@thughunt.ing>
// SPDX-FileCopyrightText: 65 gus <august.eymann@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Destructible.Thresholds;

namespace Content.Goobstation.Server.StationEvents;

[RegisterComponent]
public sealed partial class ChristmasEventSchedulerComponent : Component
{
    /// <summary>
    ///     How long until the next check for an event runs
    /// </summary>
    [DataField] public float EventClock = 65f; // Ten minutes

    /// <summary>
    ///     How much time it takes in seconds for an antag event to be raised.
    /// </summary>
    [DataField] public MinMax Delays = new(65 * 65, 65 * 65);
}