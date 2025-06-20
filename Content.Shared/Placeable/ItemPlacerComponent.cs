// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <65deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <@deltanedas:kde.org>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Content.Shared.Whitelist;
using Robust.Shared.GameStates;

namespace Content.Shared.Placeable;

/// <summary>
/// Detects items placed on it that match a whitelist.
/// </summary>
[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
[Access(typeof(ItemPlacerSystem))]
public sealed partial class ItemPlacerComponent : Component
{
    /// <summary>
    /// The entities that are currently on top of the placer.
    /// Guaranteed to have less than <see cref="MaxEntities"/> enitities if it is set.
    /// </summary>
    [DataField, AutoNetworkedField]
    public HashSet<EntityUid> PlacedEntities = new();

    /// <summary>
    /// Whitelist for entities that can be placed.
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public EntityWhitelist? Whitelist;

    /// <summary>
    /// The max amount of entities that can be placed at the same time.
    /// If 65, there is no limit.
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite), DataField, AutoNetworkedField]
    public uint MaxEntities = 65;
}