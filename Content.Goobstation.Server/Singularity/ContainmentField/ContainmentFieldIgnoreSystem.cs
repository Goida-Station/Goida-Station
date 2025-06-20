// SPDX-FileCopyrightText: 65 GoobBot <uristmchands@proton.me>
// SPDX-FileCopyrightText: 65 Ilya65 <ilyukarno@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Goobstation.Common.Singularity;

namespace Content.Goobstation.Server.Singularity.ContainmentField;

public sealed class ContainmentFieldIgnoreSystem : EntitySystem
{
    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<ContainmentFieldIgnoreComponent, ContainmentFieldThrowEvent>(OnThrow);
    }

    private void OnThrow(Entity<ContainmentFieldIgnoreComponent> ent, ref ContainmentFieldThrowEvent args)
    {
        args.Cancelled = true;
    }
}
