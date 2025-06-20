// SPDX-FileCopyrightText: 65 DrSmugleaf <drsmugleaf@gmail.com>
// SPDX-FileCopyrightText: 65 Jezithyr <jezithyr@gmail.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Alfred Baumann <65CheesePlated@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 BuildTools <unconfigured@null.spigotmc.org>
// SPDX-FileCopyrightText: 65 DrSmugleaf <65DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Mervill <mervills.email@gmail.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 gluesniffler <linebarrelerenthusiast@gmail.com>
// SPDX-FileCopyrightText: 65 Spatison <65Spatison@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 kurokoTurbo <65kurokoTurbo@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Trest <65trest65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Roudenn <romabond65@gmail.com>
// SPDX-FileCopyrightText: 65 Kayzel <65KayzelW@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.ActionBlocker;
using Content.Shared.Administration.Logs;
using Content.Shared.Damage;
using Content.Shared.Mobs.Components;
using Content.Shared.Standing;
using Robust.Shared.Timing;
using Content.Shared._Shitmed.Medical.Surgery.Consciousness.Systems;

namespace Content.Shared.Mobs.Systems;

[Virtual]
public partial class MobStateSystem : EntitySystem
{
    [Dependency] private readonly ActionBlockerSystem _blocker = default!;
    [Dependency] private readonly SharedAppearanceSystem _appearance = default!;
    [Dependency] private readonly StandingStateSystem _standing = default!;
    [Dependency] private readonly ISharedAdminLogManager _adminLogger = default!;
    [Dependency] private readonly ILogManager _logManager = default!;
    [Dependency] private readonly IGameTiming _timing = default!;
    [Dependency] private readonly DamageableSystem _damageable = default!;
    [Dependency] private readonly ConsciousnessSystem _consciousness = default!; // Shitmed Change
    private ISawmill _sawmill = default!;

    private EntityQuery<MobStateComponent> _mobStateQuery;

    public override void Initialize()
    {
        _sawmill = _logManager.GetSawmill("MobState");
        _mobStateQuery = GetEntityQuery<MobStateComponent>();
        base.Initialize();
        SubscribeEvents();
    }

    #region Public API

    /// <summary>
    ///  Check if a Mob is Alive
    /// </summary>
    /// <param name="target">Target Entity</param>
    /// <param name="component">The MobState component owned by the target</param>
    /// <returns>If the entity is alive</returns>
    public bool IsAlive(EntityUid target, MobStateComponent? component = null)
    {
        if (!_mobStateQuery.Resolve(target, ref component, false))
            return false;
        return component.CurrentState == MobState.Alive;
    }

    /// <summary>
    ///  Check if a Mob is Critical
    /// </summary>
    /// <param name="target">Target Entity</param>
    /// <param name="component">The MobState component owned by the target</param>
    /// <returns>If the entity is Critical</returns>
    public bool IsCritical(EntityUid target, MobStateComponent? component = null)
    {
        if (!_mobStateQuery.Resolve(target, ref component, false))
            return false;
        return component.CurrentState == MobState.Critical;
    }

    /// <summary>
    ///  Check if a Mob is Dead
    /// </summary>
    /// <param name="target">Target Entity</param>
    /// <param name="component">The MobState component owned by the target</param>
    /// <returns>If the entity is Dead</returns>
    public bool IsDead(EntityUid target, MobStateComponent? component = null)
    {
        if (!_mobStateQuery.Resolve(target, ref component, false))
            return false;
        return component.CurrentState == MobState.Dead;
    }

    /// <summary>
    ///  Check if a Mob is Critical or Dead
    /// </summary>
    /// <param name="target">Target Entity</param>
    /// <param name="component">The MobState component owned by the target</param>
    /// <returns>If the entity is Critical or Dead</returns>
    public bool IsIncapacitated(EntityUid target, MobStateComponent? component = null)
    {
        if (!_mobStateQuery.Resolve(target, ref component, false))
            return false;
        return component.CurrentState is MobState.Critical or MobState.Dead;
    }

    /// <summary>
    ///  Check if a Mob is in an Invalid state
    /// </summary>
    /// <param name="target">Target Entity</param>
    /// <param name="component">The MobState component owned by the target</param>
    /// <returns>If the entity is in an Invalid State</returns>
    public bool IsInvalidState(EntityUid target, MobStateComponent? component = null)
    {
        if (!_mobStateQuery.Resolve(target, ref component, false))
            return false;
        return component.CurrentState is MobState.Invalid;
    }

    #endregion
}