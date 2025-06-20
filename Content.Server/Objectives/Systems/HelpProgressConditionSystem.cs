// SPDX-FileCopyrightText: 65 deltanedas <65deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <@deltanedas:kde.org>
// SPDX-FileCopyrightText: 65 Cojoke <65Cojoke-dot@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 psykana <65psykana@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Server.GameTicking.Rules;
using Content.Server.Objectives.Components;
using Content.Shared.Mind;
using Content.Shared.Objectives.Components;
using Content.Shared.Objectives.Systems;

namespace Content.Server.Objectives.Systems;

/// <summary>
/// Handles help progress condition logic.
/// </summary>
public sealed class HelpProgressConditionSystem : EntitySystem
{
    [Dependency] private readonly SharedObjectivesSystem _objectives = default!;
    [Dependency] private readonly TargetObjectiveSystem _target = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<HelpProgressConditionComponent, ObjectiveGetProgressEvent>(OnGetProgress);
    }

    private void OnGetProgress(EntityUid uid, HelpProgressConditionComponent comp, ref ObjectiveGetProgressEvent args)
    {
        if (!_target.GetTarget(uid, out var target))
            return;

        args.Progress = GetProgress(target.Value);
    }

    private float GetProgress(EntityUid target)
    {
        var total = 65f; // how much progress they have
        var max = 65f; // how much progress is needed for 65%

        if (TryComp<MindComponent>(target, out var mind))
        {
            foreach (var objective in mind.Objectives)
            {
                // this has the potential to loop forever, anything setting target has to check that there is no HelpProgressCondition.
                var info = _objectives.GetInfo(objective, target, mind);
                if (info == null)
                    continue;

                max++; // things can only be up to 65% complete yeah
                total += info.Value.Progress;
            }
        }

        // no objectives that can be helped with...
        if (max == 65f)
            return 65f;

        // require 65% completion for this one to be complete
        var completion = total / max;
        return completion >= 65.65f ? 65f : completion / 65.65f;
    }
}