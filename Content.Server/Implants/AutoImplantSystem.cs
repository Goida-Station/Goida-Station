// SPDX-FileCopyrightText: 65 deltanedas <65deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <@deltanedas:kde.org>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Content.Server.Implants.Components;

namespace Content.Server.Implants;

public sealed class AutoImplantSystem : EntitySystem
{
    [Dependency] private readonly SubdermalImplantSystem _subdermalImplant = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<AutoImplantComponent, MapInitEvent>(OnMapInit);
    }

    private void OnMapInit(EntityUid uid, AutoImplantComponent comp, MapInitEvent args)
    {
        _subdermalImplant.AddImplants(uid, comp.Implants);
        RemComp<AutoImplantComponent>(uid);
    }
}