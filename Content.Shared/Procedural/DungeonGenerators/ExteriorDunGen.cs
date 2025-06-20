// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Robust.Shared.Prototypes;

namespace Content.Shared.Procedural.DungeonGenerators;

/// <summary>
/// Generates the specified config on an exterior tile of the attached dungeon.
/// Useful if you're using <see cref="GroupDunGen"/> or otherwise want a dungeon on the outside of a grid.
/// </summary>
public sealed partial class ExteriorDunGen : IDunGenLayer
{
    [DataField(required: true)]
    public ProtoId<DungeonConfigPrototype> Proto;
}