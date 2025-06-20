// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Kistras <65Kistras@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 ShadowCommander <65ShadowCommander@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Errant <65Errant-65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Tornado Tech <65Tornado-Technology@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 username <65whateverusername65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 whateverusername65 <whateveremail>
// SPDX-FileCopyrightText: 65 ActiveMammmoth <65ActiveMammmoth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Armok <65ARMOKS@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aviu65 <65Aviu65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 GoobBot <uristmchands@proton.me>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Mind;
using Content.Shared.Roles;
using Content.Shared.Roles.Jobs;
using Content.Shared.Store;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype.Set;

namespace Content.Server.Store.Conditions;

/// <summary>
/// Allows a store entry to be filtered out based on the user's job.
/// Supports both blacklists and whitelists
/// </summary>
public sealed partial class BuyerDepartmentCondition : ListingCondition
{
    /// <summary>
    /// A whitelist of department prototypes that can purchase this listing. Only one needs to be found.
    /// </summary>
    [DataField("whitelist", customTypeSerializer: typeof(PrototypeIdHashSetSerializer<DepartmentPrototype>))]
    public HashSet<string>? Whitelist;

    /// <summary>
    /// A blacklist of department prototypes that can purchase this listing. Only one needs to be found.
    /// </summary>
    [DataField("blacklist", customTypeSerializer: typeof(PrototypeIdHashSetSerializer<DepartmentPrototype>))]
    public HashSet<string>? Blacklist;

    public override bool Condition(ListingConditionArgs args)
    {
        var prototypeManager = IoCManager.Resolve<IPrototypeManager>();

        var ent = args.EntityManager;

        var mind = args.Buyer; // Goob start

        if (!ent.TryGetComponent<MindComponent>(args.Buyer, out var _)
        && !ent.System<SharedMindSystem>().TryGetMind(args.Buyer, out mind, out _))
            return true;

        var jobs = ent.System<SharedJobSystem>();
        jobs.MindTryGetJob(mind, out var job); //Goob end

        if (Blacklist != null && job != null)
        {
            foreach (var department in prototypeManager.EnumeratePrototypes<DepartmentPrototype>())
            {
                if (department.Roles.Contains(job.ID) && Blacklist.Contains(department.ID))
                    return false;
            }
        }

        if (Whitelist != null)
        {
            var found = false;

            if (job != null)
            {
                foreach (var department in prototypeManager.EnumeratePrototypes<DepartmentPrototype>())
                {
                    if (department.Roles.Contains(job.ID) && Whitelist.Contains(department.ID))
                    {
                        found = true;
                        break;
                    }
                }
            }

            if (!found)
                return false;
        }

        return true;
    }
}
