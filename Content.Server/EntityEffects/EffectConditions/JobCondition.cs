// SPDX-FileCopyrightText: 65 Errant <65Errant-65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 SlamBamActionman <65SlamBamActionman@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 username <65whateverusername65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 whateverusername65 <whateveremail>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using System.Linq;
using Content.Shared.EntityEffects;
using Content.Shared.Localizations;
using Content.Shared.Mind;
using Content.Shared.Mind.Components;
using Content.Shared.Roles;
using Content.Shared.Roles.Jobs;
using Robust.Shared.Prototypes;

namespace Content.Server.EntityEffects.EffectConditions;

public sealed partial class JobCondition : EntityEffectCondition
{
    [DataField(required: true)] public List<ProtoId<JobPrototype>> Job;

    public override bool Condition(EntityEffectBaseArgs args)
    {
        args.EntityManager.TryGetComponent<MindContainerComponent>(args.TargetEntity, out var mindContainer);

        if ( mindContainer is null
             || !args.EntityManager.TryGetComponent<MindComponent>(mindContainer.Mind, out var mind))
            return false;

        foreach (var roleId in mind.MindRoles)
        {
            if(!args.EntityManager.HasComponent<JobRoleComponent>(roleId))
                continue;

            if(!args.EntityManager.TryGetComponent<MindRoleComponent>(roleId, out var mindRole)
               || mindRole.JobPrototype is null)
                continue;

            if (Job.Contains(mindRole.JobPrototype.Value))
                return true;
        }

        return false;
    }

    public override string GuidebookExplanation(IPrototypeManager prototype)
    {
        var localizedNames = Job.Select(jobId => prototype.Index(jobId).LocalizedName).ToList();
        return Loc.GetString("reagent-effect-condition-guidebook-job-condition", ("job", ContentLocalizationManager.FormatListToOr(localizedNames)));
    }
}