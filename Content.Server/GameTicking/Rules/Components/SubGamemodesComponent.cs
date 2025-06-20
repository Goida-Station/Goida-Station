// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 deltanedas <65deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <@deltanedas:kde.org>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Storage;

namespace Content.Server.GameTicking.Rules.Components;

/// <summary>
/// When this gamerule is added it has a chance of adding other gamerules.
/// Since it's done when added and not when started you can still use normal start logic.
/// Used for starting subgamemodes in game presets.
/// </summary>
[RegisterComponent, Access(typeof(SubGamemodesSystem))]
public sealed partial class SubGamemodesComponent : Component
{
    /// <summary>
    /// Spawn entries for each gamerule prototype.
    /// Use orGroups if you want to limit rules.
    /// </summary>
    [DataField(required: true)]
    public List<EntitySpawnEntry> Rules = new();
}