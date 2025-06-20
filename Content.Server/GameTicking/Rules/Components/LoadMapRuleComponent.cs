// SPDX-FileCopyrightText: 65 Ed <65TheShuEd@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Kara <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 deltanedas <65deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Server.Maps;
using Content.Shared.GridPreloader.Prototypes;
using Robust.Shared.Prototypes;
using Robust.Shared.Utility;

namespace Content.Server.GameTicking.Rules.Components;

/// <summary>
/// This is used for a game rule that loads a map when activated.
/// Works with <see cref="RuleGridsComponent"/>.
/// </summary>
[RegisterComponent, Access(typeof(LoadMapRuleSystem))]
public sealed partial class LoadMapRuleComponent : Component
{
    /// <summary>
    /// A <see cref="GameMapPrototype"/> to load on a new map.
    /// </summary>
    [DataField]
    public ProtoId<GameMapPrototype>? GameMap;

    /// <summary>
    /// A map to load.
    /// </summary>
    [DataField]
    public ResPath? MapPath;

    /// <summary>
    /// A grid to load on a new map.
    /// </summary>
    [DataField]
    public ResPath? GridPath;

    /// <summary>
    /// A <see cref="PreloadedGridPrototype"/> to move to a new map.
    /// If there are no instances left nothing is done.
    /// </summary>
    [DataField]
    public ProtoId<PreloadedGridPrototype>? PreloadedGrid;
}