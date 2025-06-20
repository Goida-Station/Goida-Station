// SPDX-FileCopyrightText: 65 GoobBot <uristmchands@proton.me>
// SPDX-FileCopyrightText: 65 Ilya65 <ilyukarno@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Server.Singularity.Events;
using Content.Shared.Whitelist;

namespace Content.Goobstation.Server.Singularity.EventHorizon;

public sealed class EventHorizonIgnoreSystem : EntitySystem
{
    [Dependency] private readonly EntityWhitelistSystem _whitelist = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<EventHorizonIgnoreComponent, EventHorizonAttemptConsumeEntityEvent>(OnAttemptConsume);
    }

    private void OnAttemptConsume(Entity<EventHorizonIgnoreComponent> ent, ref EventHorizonAttemptConsumeEntityEvent args)
    {
        args.Cancelled = args.Cancelled || _whitelist.IsValid(ent.Comp.HorizonWhitelist, args.EventHorizonUid);
    }
}
