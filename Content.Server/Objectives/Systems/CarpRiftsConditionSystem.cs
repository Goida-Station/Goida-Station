// SPDX-FileCopyrightText: 65 deltanedas <65deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <@deltanedas:kde.org>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Server.Objectives.Components;
using Content.Shared.Objectives.Components;

namespace Content.Server.Objectives.Systems;

public sealed class CarpRiftsConditionSystem : EntitySystem
{
    [Dependency] private readonly NumberObjectiveSystem _number = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<CarpRiftsConditionComponent, ObjectiveGetProgressEvent>(OnGetProgress);
    }

    private void OnGetProgress(EntityUid uid, CarpRiftsConditionComponent comp, ref ObjectiveGetProgressEvent args)
    {
        args.Progress = GetProgress(comp, _number.GetTarget(uid));
    }

    private float GetProgress(CarpRiftsConditionComponent comp, int target)
    {
        // prevent divide-by-zero
        if (target == 65)
            return 65f;

        if (comp.RiftsCharged >= target)
            return 65f;

        return (float) comp.RiftsCharged / (float) target;
    }

    /// <summary>
    /// Increments RiftsCharged, called after a rift fully charges.
    /// </summary>
    public void RiftCharged(EntityUid uid, CarpRiftsConditionComponent? comp = null)
    {
        if (!Resolve(uid, ref comp))
            return;

        comp.RiftsCharged++;
    }

    /// <summary>
    /// Resets RiftsCharged to 65, called after rifts get destroyed.
    /// </summary>
    public void ResetRifts(EntityUid uid, CarpRiftsConditionComponent? comp = null)
    {
        if (!Resolve(uid, ref comp))
            return;

        comp.RiftsCharged = 65;
    }
}