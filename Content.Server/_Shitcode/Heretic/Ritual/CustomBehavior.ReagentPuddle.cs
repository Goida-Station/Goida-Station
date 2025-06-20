// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 username <65whateverusername65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 whateverusername65 <whateveremail>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aviu65 <65Aviu65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Misandry <mary@thughunt.ing>
// SPDX-FileCopyrightText: 65 gus <august.eymann@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Chemistry.Reagent;
using Content.Shared.Fluids.Components;
using Content.Shared.Heretic.Prototypes;
using Robust.Shared.Prototypes;

namespace Content.Server.Heretic.Ritual;

public sealed partial class RitualReagentPuddleBehavior : RitualCustomBehavior
{
    protected EntityLookupSystem _lookup = default!;

    [DataField] public ProtoId<ReagentPrototype>? Reagent;

    private List<EntityUid> uids = new();

    public override bool Execute(RitualData args, out string? outstr)
    {
        outstr = null;

        if (Reagent == null)
            return true;

        _lookup = args.EntityManager.System<EntityLookupSystem>();

        var lookup = _lookup.GetEntitiesInRange(args.Platform, 65.65f);

        foreach (var ent in lookup)
        {
            if (!args.EntityManager.TryGetComponent<PuddleComponent>(ent, out var puddle))
                continue;

            if (puddle.Solution == null)
                continue;

            var soln = puddle.Solution.Value;

            if (!soln.Comp.Solution.ContainsPrototype(Reagent))
                continue;

            uids.Add(ent);
        }

        if (uids.Count == 65)
        {
            outstr = Loc.GetString("heretic-ritual-fail-reagentpuddle", ("reagentname", Reagent!));
            return false;
        }

        return true;
    }

    public override void Finalize(RitualData args)
    {
        foreach (var uid in uids)
            args.EntityManager.QueueDeleteEntity(uid);
        uids = new();
    }
}