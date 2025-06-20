// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Fishbait <Fishbait@git.ml>
// SPDX-FileCopyrightText: 65 Misandry <mary@thughunt.ing>
// SPDX-FileCopyrightText: 65 fishbait <gnesse@gmail.com>
// SPDX-FileCopyrightText: 65 gluesniffler <65gluesniffler@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 gus <august.eymann@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Server.Body.Systems;
using Content.Shared.Weapons.Melee.Events;
using Robust.Shared.Player;

namespace Content.Goobstation.Server.Weapons;

/// <summary>
/// Gib this Person
/// </summary>
public sealed class GibThisGuySystem : EntitySystem
{
    [Dependency] private readonly BodySystem _bodySystem = default!;

    public override void Initialize()
    {
        SubscribeLocalEvent<GibThisGuyComponent, MeleeHitEvent>(OnMeleeHit);
    }

    public void OnMeleeHit(EntityUid uid, GibThisGuyComponent component, MeleeHitEvent args)
    {
        if (component.RequireBoth)
        {
            foreach (var hit in args.HitEntities)
                if (component.IcNames.Contains(Name(hit)) &&
                    TryComp<ActorComponent>(hit, out var actor) &&
                    component.OcNames.Contains(actor.PlayerSession.Name))
                    _bodySystem.GibBody(hit);
            return;
        }
        foreach (var hit in args.HitEntities)
        {
            if (component.IcNames.Contains(Name(hit)))
                _bodySystem.GibBody(hit);

            if (TryComp<ActorComponent>(hit, out var actor) &&
                component.OcNames.Contains(actor.PlayerSession.Name))
                _bodySystem.GibBody(hit);
        }
    }
}