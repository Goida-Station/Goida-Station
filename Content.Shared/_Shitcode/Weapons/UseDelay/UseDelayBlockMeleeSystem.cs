// SPDX-FileCopyrightText: 65 Aviu65 <65Aviu65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Remuchi <65Remuchi@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 VMSolidus <evilexecutive@gmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Misandry <mary@thughunt.ing>
// SPDX-FileCopyrightText: 65 gus <august.eymann@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using System.Linq;
using Content.Shared.Timing;
using Content.Shared.Weapons.Melee.Events;

namespace Content.Shared._Goobstation.Weapons.UseDelay;

public sealed class UseDelayBlockMeleeSystem : EntitySystem
{
    [Dependency] private readonly UseDelaySystem _useDelay = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<UseDelayBlockMeleeComponent, AttemptMeleeEvent>(OnMeleeAttempt);
    }

    private void OnMeleeAttempt(Entity<UseDelayBlockMeleeComponent> ent, ref AttemptMeleeEvent args)
    {
        if (!TryComp(ent, out UseDelayComponent? useDelay))
            return;

        if (ent.Comp.Delays.Any(delay => _useDelay.IsDelayed((ent, useDelay), delay)))
            args.Cancelled = true;
    }
}