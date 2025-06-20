// SPDX-FileCopyrightText: 65 deltanedas <65deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <@deltanedas:kde.org>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Clothing.EntitySystems;
using Content.Shared.NPC.Prototypes;
using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;

namespace Content.Shared.Clothing.Components;

/// <summary>
/// When equipped, adds the wearer to a faction.
/// When removed, removes the wearer from a faction.
/// </summary>
[RegisterComponent, NetworkedComponent, Access(typeof(FactionClothingSystem))]
public sealed partial class FactionClothingComponent : Component
{
    /// <summary>
    /// Faction to add and remove.
    /// </summary>
    [DataField(required: true)]
    public ProtoId<NpcFactionPrototype> Faction = string.Empty;

    /// <summary>
    /// If true, the wearer was already part of the faction.
    /// This prevents wrongly removing them after removing the item.
    /// </summary>
    [DataField]
    public bool AlreadyMember;
}