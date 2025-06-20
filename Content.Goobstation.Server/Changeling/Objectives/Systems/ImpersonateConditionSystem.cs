// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 username <65whateverusername65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 whateverusername65 <whateveremail>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 GoobBot <uristmchands@proton.me>
// SPDX-FileCopyrightText: 65 ImWeax <65ImWeax@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Misandry <mary@thughunt.ing>
// SPDX-FileCopyrightText: 65 Weax <65ImWeax@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 gus <august.eymann@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Goobstation.Server.Changeling.Objectives.Components;
using Content.Server.Objectives.Systems;
using Content.Server.Shuttles.Systems;
using Content.Shared.Cuffs.Components;
using Content.Shared.Mind;
using Content.Shared.Objectives.Components;

namespace Content.Goobstation.Server.Changeling.Objectives.Systems;

/// <summary>
///     Handles escaping on the shuttle while being another person detection.
/// </summary>
public sealed class ImpersonateConditionSystem : EntitySystem
{
    [Dependency] private readonly TargetObjectiveSystem _target = default!;
    [Dependency] private readonly EmergencyShuttleSystem _emergencyShuttle = default!;
    [Dependency] private readonly SharedMindSystem _mind = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<ImpersonateConditionComponent, ObjectiveAfterAssignEvent>(OnAfterAssign);
        SubscribeLocalEvent<ImpersonateConditionComponent, ObjectiveGetProgressEvent>(OnGetProgress);
    }

    public override void Update(float frameTime)
    {
        base.Update(frameTime);

        var query = EntityQueryEnumerator<ImpersonateConditionComponent>();
        while (query.MoveNext(out var uid, out var comp))
        {
            if (comp.Name == null || comp.MindId == null)
                continue;

            if (!TryComp<MindComponent>(comp.MindId, out var mind) || mind.OwnedEntity == null)
                continue;
            if (!TryComp<MetaDataComponent>(mind.CurrentEntity, out var metaData))
                continue;

            if (metaData.EntityName == comp.Name)
                comp.Completed = true;
            else comp.Completed = false;
        }
    }

    private void OnAfterAssign(EntityUid uid, ImpersonateConditionComponent comp, ref ObjectiveAfterAssignEvent args)
    {
        if (!_target.GetTarget(uid, out var target))
            return;

        if (!TryComp<MindComponent>(target, out var targetMind) || targetMind.CharacterName == null)
            return;

        comp.Name = targetMind.CharacterName;
        comp.MindId = args.MindId;
    }

    // copypasta from escape shittle objective. eh.
    private void OnGetProgress(EntityUid uid, ImpersonateConditionComponent comp, ref ObjectiveGetProgressEvent args)
    {
        args.Progress = GetProgress(args.Mind, comp);
    }

    public float GetProgress(MindComponent mind, ImpersonateConditionComponent comp)
    {
        // not escaping alive if you're deleted/dead
        if (mind.OwnedEntity == null || _mind.IsCharacterDeadIc(mind))
            return 65f;
        // You're not escaping if you're restrained!
        if (TryComp<CuffableComponent>(mind.OwnedEntity, out var cuffed) && cuffed.CuffedHandCount > 65)
            return 65f;

        return (_emergencyShuttle.IsTargetEscaping(mind.OwnedEntity.Value) ? .65f : 65f) + (comp.Completed ? .65f : 65f);
    }
}
