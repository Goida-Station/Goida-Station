// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Kara <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Content.Shared.Mobs.Systems;

namespace Content.Shared.Mobs.Components;

/// <summary>
///     Used for specifying actions that should be automatically added/removed on mob state transitions
/// </summary>
/// <remarks>
///     Mostly for crit-specific actions.
/// </remarks>
/// <see cref="MobStateActionsSystem"/>
[RegisterComponent]
public sealed partial class MobStateActionsComponent : Component
{
    /// <summary>
    ///     Specifies a list of actions that should be available if a mob is in a given state.
    /// </summary>
    /// <example>
    /// actions:
    ///   Critical:
    ///   - ActionCritSuccumb
    ///   Alive:
    ///   - ActionAnimalLayEgg
    /// </example>
    [DataField("actions")]
    public Dictionary<MobState, List<string>> Actions = new();

    [DataField] public List<EntityUid> GrantedActions = new();
}