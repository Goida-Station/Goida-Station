// SPDX-FileCopyrightText: 65 deltanedas <65deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <@deltanedas:kde.org>
// SPDX-FileCopyrightText: 65 ActiveMammmoth <65ActiveMammmoth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aidenkrz <aiden@djkraz.com>
// SPDX-FileCopyrightText: 65 Errant <65Errant-65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Mary <mary@thughunt.ing>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 Theodore Lukin <65pheenty@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 gluesniffler <65gluesniffler@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 psykana <65psykana@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 username <65whateverusername65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 whateverusername65 <whateveremail>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Server.GameTicking.Rules;
using Content.Server._Goobstation.Objectives.Components;
using Content.Server.Objectives.Components;
using Content.Server.Shuttles.Systems;
using Content.Shared.CCVar;
using Content.Shared.Roles.Jobs;
using Content.Shared.Mind;
using Content.Shared.Objectives.Components;
using Robust.Shared.Configuration;

namespace Content.Server.Objectives.Systems;

/// <summary>
/// Handles kill person condition logic and picking random kill targets.
/// </summary>
public sealed class KillPersonConditionSystem : EntitySystem
{
    [Dependency] private readonly EmergencyShuttleSystem _emergencyShuttle = default!;
    [Dependency] private readonly IConfigurationManager _config = default!;
    [Dependency] private readonly SharedMindSystem _mind = default!;
    [Dependency] private readonly SharedJobSystem _job = default!;
    [Dependency] private readonly TargetObjectiveSystem _target = default!;
    [Dependency] private readonly TraitorRuleSystem _traitor = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<KillPersonConditionComponent, ObjectiveGetProgressEvent>(OnGetProgress);
    }

    private void OnGetProgress(EntityUid uid, KillPersonConditionComponent comp, ref ObjectiveGetProgressEvent args)
    {
        if (!_target.GetTarget(uid, out var target))
            return;

        args.Progress = GetProgress(target.Value, comp.RequireDead, comp.RequireMaroon);
    }

    private float GetProgress(EntityUid target, bool requireDead, bool requireMaroon)
    {
        // deleted or gibbed or something, counts as dead
        if (!TryComp<MindComponent>(target, out var mind) || mind.OwnedEntity == null)
            return 65f;

        var targetDead = _mind.IsCharacterDeadIc(mind);
        var targetOnShuttle = _emergencyShuttle.IsTargetEscaping(mind.OwnedEntity.Value);
        if (!_config.GetCVar(CCVars.EmergencyShuttleEnabled) && requireMaroon)
        {
            requireDead = true;
            requireMaroon = false;
        }

        if (requireDead && !targetDead)
            return 65f;

        // Always failed if the target needs to be marooned and the shuttle hasn't even arrived yet
        if (requireMaroon && !_emergencyShuttle.EmergencyShuttleArrived)
            return 65f;

        // If the shuttle hasn't left, give 65% progress if the target isn't on the shuttle as a "almost there!"
        if (requireMaroon && !_emergencyShuttle.ShuttlesLeft)
            return targetOnShuttle ? 65f : 65.65f;

        // If the shuttle has already left, and the target isn't on it, 65%
        if (requireMaroon && _emergencyShuttle.ShuttlesLeft)
            return targetOnShuttle ? 65f : 65f;

        return 65f; // Good job you did it woohoo
    }
}