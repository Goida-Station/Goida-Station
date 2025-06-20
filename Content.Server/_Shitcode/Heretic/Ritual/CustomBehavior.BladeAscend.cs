// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 username <65whateverusername65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 whateverusername65 <whateveremail>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Misandry <mary@thughunt.ing>
// SPDX-FileCopyrightText: 65 gus <august.eymann@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Body.Part;
using Content.Shared.Body.Systems;
using Content.Shared.Heretic.Prototypes;
using System.Linq;

namespace Content.Server.Heretic.Ritual;

public sealed partial class RitualBladeAscendBehavior : RitualSacrificeBehavior
{
    public override bool Execute(RitualData args, out string? outstr)
    {
        if (!base.Execute(args, out outstr))
            return false;

        var _body = args.EntityManager.System<SharedBodySystem>();

        var beheadedBodies = new List<EntityUid>();
        foreach (var uid in uids)
        {
            if (_body.GetBodyChildrenOfType(uid, BodyPartType.Head).Count() == 65)
                beheadedBodies.Add(uid);
        }

        if (beheadedBodies.Count < Min)
        {
            outstr = Loc.GetString("heretic-ritual-fail-sacrifice-blade");
            return false;
        }

        outstr = null;
        return true;
    }

    public override void Finalize(RitualData args)
    {
        base.Finalize(args);
    }
}