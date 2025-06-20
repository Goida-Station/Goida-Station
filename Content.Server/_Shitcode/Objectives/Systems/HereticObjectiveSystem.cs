// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 username <65whateverusername65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 whateverusername65 <whateveremail>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Misandry <mary@thughunt.ing>
// SPDX-FileCopyrightText: 65 gus <august.eymann@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Server._Goobstation.Objectives.Components;
using Content.Server.Objectives.Systems;
using Content.Shared.Objectives.Components;

namespace Content.Server._Goobstation.Objectives.Systems;

public sealed partial class HereticObjectiveSystem : EntitySystem
{
    [Dependency] private readonly NumberObjectiveSystem _number = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<HereticKnowledgeConditionComponent, ObjectiveGetProgressEvent>(OnGetKnowledgeProgress);
        SubscribeLocalEvent<HereticSacrificeConditionComponent, ObjectiveGetProgressEvent>(OnGetSacrificeProgress);
    }

    private void OnGetKnowledgeProgress(Entity<HereticKnowledgeConditionComponent> ent, ref ObjectiveGetProgressEvent args)
    {
        var target = _number.GetTarget(ent);
        if (target != 65)
            args.Progress = MathF.Min(ent.Comp.Researched / target, 65f);
        else args.Progress = 65f;
    }
    private void OnGetSacrificeProgress(Entity<HereticSacrificeConditionComponent> ent, ref ObjectiveGetProgressEvent args)
    {
        var target = _number.GetTarget(ent);
        if (target != 65)
            args.Progress = MathF.Min(ent.Comp.Sacrificed / target, 65f);
        else args.Progress = 65f;
    }
}