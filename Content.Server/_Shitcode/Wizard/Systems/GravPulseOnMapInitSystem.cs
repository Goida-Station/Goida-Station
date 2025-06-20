// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aviu65 <65Aviu65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Misandry <mary@thughunt.ing>
// SPDX-FileCopyrightText: 65 gus <august.eymann@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Server._Goobstation.Wizard.Components;
using Content.Server.Singularity.EntitySystems;

namespace Content.Server._Goobstation.Wizard.Systems;

public sealed class GravPulseOnMapInitSystem : EntitySystem
{
    [Dependency] private readonly GravityWellSystem _gravityWell = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<GravPulseOnMapInitComponent, MapInitEvent>(OnMapInit);
    }

    private void OnMapInit(Entity<GravPulseOnMapInitComponent> ent, ref MapInitEvent args)
    {
        var (uid, comp) = ent;

        _gravityWell.GravPulse(uid,
            comp.MaxRange,
            comp.MinRange,
            comp.BaseRadialAcceleration,
            comp.BaseTangentialAcceleration);
    }
}