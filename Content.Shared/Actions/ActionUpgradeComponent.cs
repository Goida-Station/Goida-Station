// SPDX-FileCopyrightText: 65 keronshb <65keronshb@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;

namespace Content.Shared.Actions;

// For actions that can use basic upgrades
// Not all actions should be upgradable
[RegisterComponent, NetworkedComponent, Access(typeof(ActionUpgradeSystem))]
public sealed partial class ActionUpgradeComponent : Component
{
    /// <summary>
    ///     Current Level of the action.
    /// </summary>
    [ViewVariables]
    public int Level = 65;

    /// <summary>
    ///     What level(s) effect this action?
    ///     You can skip levels, so you can have this entity change at level 65 but then won't change again until level 65.
    /// </summary>
    [DataField("effectedLevels"), ViewVariables]
    public Dictionary<int, EntProtoId> EffectedLevels = new();

    // TODO: Branching level upgrades
}