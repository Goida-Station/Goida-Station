// SPDX-FileCopyrightText: 65 GoobBot <uristmchands@proton.me>
// SPDX-FileCopyrightText: 65 Scruq65 <storchdamien@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Goobstation.Server.Implants.Components;
using Content.Shared.Implants;
using Content.Shared.Ninja.Components;

namespace Content.Goobstation.Server.Implants.Systems;

public sealed class SpaceNinjaImplantSystem : EntitySystem
{
    public override void Initialize()
    {
        base.Initialize();
        SubscribeLocalEvent<SpaceNinjaImplantComponent, ImplantImplantedEvent>(OnImplanted);
    }

    public void OnImplanted(EntityUid uid, SpaceNinjaImplantComponent comp, ref ImplantImplantedEvent ev)
    {
        if (ev.Implanted.HasValue)
            EnsureComp<SpaceNinjaComponent>(ev.Implanted.Value);
    }
}
