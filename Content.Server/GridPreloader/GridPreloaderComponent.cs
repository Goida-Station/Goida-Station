// SPDX-FileCopyrightText: 65 Ed <65TheShuEd@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Kara <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.GridPreloader.Prototypes;
using Robust.Shared.Prototypes;

namespace Content.Server.GridPreloader;

/// <summary>
/// Component storing data about preloaded grids and their location
/// Goes on the map entity
/// </summary>
[RegisterComponent, Access(typeof(GridPreloaderSystem))]
public sealed partial class GridPreloaderComponent : Component
{
    [DataField]
    public Dictionary<ProtoId<PreloadedGridPrototype>, List<EntityUid>> PreloadedGrids = new();
}