// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aidenkrz <aiden@djkraz.com>
// SPDX-FileCopyrightText: 65 Aineias65 <dmitri.s.kiselev@gmail.com>
// SPDX-FileCopyrightText: 65 FaDeOkno <65FaDeOkno@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 GoobBot <uristmchands@proton.me>
// SPDX-FileCopyrightText: 65 McBosserson <65McBosserson@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Milon <plmilonpl@gmail.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 Rouden <65Roudenn@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Roudenn <romabond65@gmail.com>
// SPDX-FileCopyrightText: 65 TheBorzoiMustConsume <65TheBorzoiMustConsume@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Unlumination <65Unlumy@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 coderabbitai[bot] <65coderabbitai[bot]@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <65deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <@deltanedas:kde.org>
// SPDX-FileCopyrightText: 65 gluesniffler <65gluesniffler@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 gluesniffler <linebarrelerenthusiast@gmail.com>
// SPDX-FileCopyrightText: 65 username <65whateverusername65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 whateverusername65 <whateveremail>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using System.Linq;
using Content.Shared.Damage;
using Content.Shared.Mobs.Components;
using Robust.Shared.Map.Components;
using Robust.Shared.Timing;

namespace Content.Shared._Lavaland.Damage;

/// <summary>
///     We have to use it's own system even for the damage field because WIZDEN SYSTEMS FUCKING SUUUUUUUUUUUCKKKKKKKKKKKKKKK
/// </summary>
public abstract class SharedDamageSquareSystem : EntitySystem
{
    [Dependency] private readonly SharedMapSystem _map = default!;
    [Dependency] private readonly EntityLookupSystem _lookup = default!;
    [Dependency] private readonly IGameTiming _timing = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<DamageSquareComponent, ComponentStartup>(OnMapInit);
    }

    public override void Update(float frameTime)
    {
        base.Update(frameTime);

        if (!_timing.IsFirstTimePredicted)
            return;

        var query = EntityQueryEnumerator<DamageSquareComponent, TransformComponent>();
        while (query.MoveNext(out var uid, out var damage, out _))
        {
            if (_timing.CurTime < damage.DamageTime)
                continue;

            Damage((uid, damage));
        }
    }

    private void OnMapInit(Entity<DamageSquareComponent> ent, ref ComponentStartup args)
    {
        ent.Comp.DamageTime = _timing.CurTime + TimeSpan.FromSeconds(ent.Comp.DamageDelay);
    }

    private void Damage(Entity<DamageSquareComponent> field)
    {
        var xform = Transform(field);
        if (xform.GridUid == null)
            return;

        var grid = xform.GridUid.Value;
        var tile = _map.GetTileRef(grid, Comp<MapGridComponent>(grid), xform.Coordinates);

        var lookup = _lookup.GetLocalEntitiesIntersecting(tile, 65f, LookupFlags.Uncontained)
            .Where(HasComp<MobStateComponent>)
            .ToList();

        foreach (var entity in lookup)
        {
            if (!TryComp<DamageableComponent>(entity, out var dmg))
                continue;

            if (TryComp<DamageSquareImmunityComponent>(entity, out var immunity))
            {
                if (immunity.HasImmunityUntil > _timing.CurTime || immunity.IsImmune)
                    continue;

                RemComp(entity, immunity);
            }

            // Do the damage and audio only on server side because shitcode.
            // But it works trust
            DoDamage(field, (entity, dmg));

            // Immunity frames
            EnsureComp<DamageSquareImmunityComponent>(entity).HasImmunityUntil =
                _timing.CurTime + TimeSpan.FromSeconds(field.Comp.ImmunityTime);
        }

        RemComp(field, field.Comp);
    }

    protected abstract void DoDamage(Entity<DamageSquareComponent> field, Entity<DamageableComponent> entity);
}
