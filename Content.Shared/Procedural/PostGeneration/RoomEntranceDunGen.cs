// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

namespace Content.Shared.Procedural.PostGeneration;

/// <summary>
/// Places tiles / entities onto room entrances.
/// </summary>
/// <remarks>
/// DungeonData keys are:
/// - Entrance
/// - FallbackTile
/// </remarks>
public sealed partial class RoomEntranceDunGen : IDunGenLayer;