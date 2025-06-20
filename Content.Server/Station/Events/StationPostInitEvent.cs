// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 ElectroJr <leonsfriedrich@gmail.com>
// SPDX-FileCopyrightText: 65 Kara <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Content.Server.Station.Components;

namespace Content.Server.Station.Events;

/// <summary>
/// Raised directed on a station after it has been initialized, as well as broadcast.
/// This gets raised after the entity has been map-initialized, and the station's centcomm map/entity (if any) has been
/// set up.
/// </summary>
[ByRefEvent]
public readonly record struct StationPostInitEvent(Entity<StationDataComponent> Station);