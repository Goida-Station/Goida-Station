// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Ed <65TheShuEd@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <65deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <@deltanedas:kde.org>
// SPDX-FileCopyrightText: 65 faint <65ficcialfaint@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <comedian_vs_clown@hotmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Content.Shared.NPC.Systems;
using Robust.Shared.GameStates;

namespace Content.Shared.NPC.Components;

/// <summary>
/// Prevents an NPC from attacking ignored entities from enemy factions.
/// Can be added to if pettable, see PettableFriendComponent.
/// </summary>
[RegisterComponent, NetworkedComponent, Access(typeof(NpcFactionSystem), typeof(SharedNPCImprintingOnSpawnBehaviourSystem))] // TO DO (Metalgearsloth): If we start adding a billion access overrides they should be going through a system as then there's no reason to have access, but I'll fix this when I rework npcs.
public sealed partial class FactionExceptionComponent : Component
{
    /// <summary>
    /// Collection of entities that this NPC will refuse to attack
    /// </summary>
    [DataField]
    public HashSet<EntityUid> Ignored = new();

    /// <summary>
    /// Collection of entities that this NPC will attack, regardless of faction.
    /// </summary>
    [DataField]
    public HashSet<EntityUid> Hostiles = new();
}