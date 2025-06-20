// SPDX-FileCopyrightText: 65 Aviu65 <65Aviu65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Remuchi <65Remuchi@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 VMSolidus <evilexecutive@gmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Misandry <mary@thughunt.ing>
// SPDX-FileCopyrightText: 65 gus <august.eymann@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Timing;
using Content.Shared.Weapons.Ranged.Systems;

namespace Content.Shared._Goobstation.Weapons.UseDelay;

public sealed class UseDelayBlockShootSystem : EntitySystem
{
    [Dependency] private readonly UseDelaySystem _useDelay = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<UseDelayBlockShootComponent, AttemptShootEvent>(OnShootAttempt);
    }

    private void OnShootAttempt(Entity<UseDelayBlockShootComponent> ent, ref AttemptShootEvent args)
    {
        if (TryComp(ent, out UseDelayComponent? useDelay) && _useDelay.IsDelayed((ent, useDelay)))
            args.Cancelled = true;
    }
}