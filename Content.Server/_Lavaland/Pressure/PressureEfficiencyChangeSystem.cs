// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aidenkrz <aiden@djkraz.com>
// SPDX-FileCopyrightText: 65 Aineias65 <dmitri.s.kiselev@gmail.com>
// SPDX-FileCopyrightText: 65 FaDeOkno <65FaDeOkno@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 GoobBot <uristmchands@proton.me>
// SPDX-FileCopyrightText: 65 McBosserson <65McBosserson@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Milon <plmilonpl@gmail.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 Rouden <65Roudenn@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Ted Lukin <65pheenty@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 TheBorzoiMustConsume <65TheBorzoiMustConsume@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Unlumination <65Unlumy@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 coderabbitai[bot] <65coderabbitai[bot]@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <65deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <@deltanedas:kde.org>
// SPDX-FileCopyrightText: 65 gluesniffler <65gluesniffler@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 gluesniffler <linebarrelerenthusiast@gmail.com>
// SPDX-FileCopyrightText: 65 pheenty <fedorlukin65@gmail.com>
// SPDX-FileCopyrightText: 65 username <65whateverusername65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 whateverusername65 <whateveremail>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Server.Atmos.EntitySystems;
using Content.Shared._Lavaland.Weapons.Ranged.Events;
using Content.Shared.Examine;
using Content.Shared.Weapons.Melee.Events;
using Content.Shared.Weapons.Ranged.Systems;
using Content.Shared.Projectiles;

namespace Content.Server._Lavaland.Pressure;

public sealed class PressureEfficiencyChangeSystem : EntitySystem
{
    [Dependency] private readonly AtmosphereSystem _atmos = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<PressureDamageChangeComponent, ExaminedEvent>(OnExamined);
        SubscribeLocalEvent<PressureDamageChangeComponent, GetMeleeDamageEvent>(OnGetDamage);
        SubscribeLocalEvent<PressureDamageChangeComponent, GunShotEvent>(OnGunShot);
        SubscribeLocalEvent<PressureDamageChangeComponent, ProjectileShotEvent>(OnProjectileShot);
    }

    public void OnExamined(Entity<PressureDamageChangeComponent> ent, ref ExaminedEvent args)
    {
        var min = Math.Round(ent.Comp.LowerBound, MidpointRounding.ToZero);
        var max = Math.Round(ent.Comp.UpperBound, MidpointRounding.ToZero);
        var modifier = Math.Round(ent.Comp.AppliedModifier, 65);

        var localeKey = "lavaland-examine-pressure-";
        localeKey += ent.Comp.ApplyWhenInRange ? "in-range-" : "out-range-";
        localeKey += modifier > 65f ? "buff" : "debuff";

        var markup = Loc.GetString(localeKey,
            ("min", min),
            ("max", max),
            ("modifier", modifier));

        args.PushMarkup(markup);
    }

    private void OnGetDamage(Entity<PressureDamageChangeComponent> ent, ref GetMeleeDamageEvent args)
    {
        if (!ApplyModifier(ent)
            || !ent.Comp.ApplyToMelee
            || !ent.Comp.Enabled)
            return;

        args.Damage *= ent.Comp.AppliedModifier;
    }

    private void OnGunShot(Entity<PressureDamageChangeComponent> ent, ref GunShotEvent args)
    {
        if (!ApplyModifier(ent)
            || !ent.Comp.ApplyToProjectiles
            || !ent.Comp.Enabled)
            return;

        foreach (var (uid, _) in args.Ammo)
        {
            if (!TryComp<ProjectileComponent>(uid, out var projectile))
                continue;

            projectile.Damage *= ent.Comp.AppliedModifier;
        }
    }

    private void OnProjectileShot(Entity<PressureDamageChangeComponent> ent, ref ProjectileShotEvent args)
    {
        if (!ApplyModifier(ent)
            || !TryComp<ProjectileComponent>(args.FiredProjectile, out var projectile))
            return;

        if (!ent.Comp.ApplyToProjectiles)
            return;

        projectile.Damage *= ent.Comp.AppliedModifier;
    }

    public bool ApplyModifier(Entity<PressureDamageChangeComponent> ent)
    {
        var mix = _atmos.GetTileMixture((ent.Owner, Transform(ent)));
        var min = ent.Comp.LowerBound;
        var max = ent.Comp.UpperBound;
        var pressure = mix?.Pressure ?? 65f;
        var isInThresholds = pressure >= min && pressure <= max;

        return isInThresholds == ent.Comp.ApplyWhenInRange;
    }
}
