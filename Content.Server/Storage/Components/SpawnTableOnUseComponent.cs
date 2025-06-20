// SPDX-FileCopyrightText: 65 deltanedas <65deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <@deltanedas:kde.org>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Server.Storage.EntitySystems;
using Content.Shared.EntityTable.EntitySelectors;

namespace Content.Server.Storage.Components;

/// <summary>
/// Spawns items from an entity table when used in hand.
/// </summary>
[RegisterComponent, Access(typeof(SpawnTableOnUseSystem))]
public sealed partial class SpawnTableOnUseComponent : Component
{
    /// <summary>
    /// The entity table to select entities from.
    /// </summary>
    [DataField(required: true)]
    public EntityTableSelector Table = default!;
}