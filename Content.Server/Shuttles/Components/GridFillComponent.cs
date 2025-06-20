// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Ygg65 <y.laughing.man.y@gmail.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 AJCM <AJCM@tutanota.com>
// SPDX-FileCopyrightText: 65 Rainfall <rainfey65git@gmail.com>
// SPDX-FileCopyrightText: 65 Rainfey <rainfey65github@gmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Content.Server.Shuttles.Systems;
using Robust.Shared.Prototypes;
using Robust.Shared.Utility;

namespace Content.Server.Shuttles.Components;

/// <summary>
/// If added to an airlock will try to autofill a grid onto it on MapInit
/// </summary>
[RegisterComponent, Access(typeof(ShuttleSystem))]
public sealed partial class GridFillComponent : Component
{
    [DataField]
    public ResPath Path = new("/Maps/Shuttles/escape_pod_small.yml");

    /// <summary>
    /// Components to be added to any spawned grids.
    /// </summary>
    [DataField]
    public ComponentRegistry AddComponents = new();
}