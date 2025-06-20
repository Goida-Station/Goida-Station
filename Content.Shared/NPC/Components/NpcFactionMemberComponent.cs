// SPDX-FileCopyrightText: 65 deltanedas <65deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <@deltanedas:kde.org>
// SPDX-FileCopyrightText: 65 lzk <65lzk65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <comedian_vs_clown@hotmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.NPC.Prototypes;
using Content.Shared.NPC.Systems;
using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;

namespace Content.Shared.NPC.Components;

[RegisterComponent, NetworkedComponent, Access(typeof(NpcFactionSystem))]
public sealed partial class NpcFactionMemberComponent : Component
{
    /// <summary>
    /// Factions this entity is a part of.
    /// </summary>
    [DataField]
    public HashSet<ProtoId<NpcFactionPrototype>> Factions = new();

    /// <summary>
    /// Cached friendly factions.
    /// </summary>
    [ViewVariables]
    public readonly HashSet<ProtoId<NpcFactionPrototype>> FriendlyFactions = new();

    /// <summary>
    /// Cached hostile factions.
    /// </summary>
    [ViewVariables]
    public readonly HashSet<ProtoId<NpcFactionPrototype>> HostileFactions = new();

    /// <summary>
    /// Used to add friendly factions in prototypes.
    /// </summary>
    [DataField, ViewVariables]
    public HashSet<ProtoId<NpcFactionPrototype>>? AddFriendlyFactions;

    /// <summary>
    /// Used to add hostile factions in prototypes.
    /// </summary>
    [DataField, ViewVariables]
    public HashSet<ProtoId<NpcFactionPrototype>>? AddHostileFactions;
}