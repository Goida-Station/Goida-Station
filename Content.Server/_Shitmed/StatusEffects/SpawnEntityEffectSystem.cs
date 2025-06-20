// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 gluesniffler <65gluesniffler@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.NPC.Components;
using Content.Shared.NPC.Systems;
using Content.Shared._Shitmed.StatusEffects;

namespace Content.Server._Shitmed.StatusEffects;

public sealed class SpawnEntityEffectSystem : EntitySystem
{
    [Dependency] private readonly SharedTransformSystem _xformSys = default!;
    [Dependency] private readonly NpcFactionSystem _factionException = default!;

    public override void Initialize()
    {
        SubscribeLocalEvent<SpawnSpiderEggsComponent, ComponentInit>(OnInit);
        SubscribeLocalEvent<SpawnSlimesComponent, ComponentInit>(OnInit);
        SubscribeLocalEvent<SpawnEmpComponent, ComponentInit>(OnInit);
        SubscribeLocalEvent<SpawnGravityWellComponent, ComponentInit>(OnInit);
        SubscribeLocalEvent<SpawnFlashComponent, ComponentInit>(OnInit);
        SubscribeLocalEvent<SpawnSmokeComponent, ComponentInit>(OnInit);
    }

    private void OnInit(EntityUid uid, SpawnEntityEffectComponent component, ComponentInit args)
    {
        EntityUid entity;

        if (component.AttachToParent)
        {
            entity = SpawnAttachedTo(component.EntityPrototype, Transform(uid).Coordinates);
            _xformSys.SetParent(entity, uid);
        }
        else
        {
            entity = Spawn(component.EntityPrototype, Transform(uid).Coordinates);
        }

        if (component.IsFriendly)
        {
            if (EnsureComp<FactionExceptionComponent>(entity, out var comp))
                return;

            _factionException.IgnoreEntities(entity, new[] { uid });
        }

    }


}