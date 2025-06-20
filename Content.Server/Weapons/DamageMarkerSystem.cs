// SPDX-FileCopyrightText: 65 AJCM-git <65AJCM-git@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aidenkrz <aiden@djkraz.com>
// SPDX-FileCopyrightText: 65 Aineias65 <dmitri.s.kiselev@gmail.com>
// SPDX-FileCopyrightText: 65 FaDeOkno <65FaDeOkno@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 GoobBot <uristmchands@proton.me>
// SPDX-FileCopyrightText: 65 McBosserson <65McBosserson@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Milon <plmilonpl@gmail.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 Rouden <65Roudenn@users.noreply.github.com>
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

using Content.Shared.Weapons.Marker;
// Lavaland Change
using Content.Server._Lavaland.Pressure;
using Content.Shared._Lavaland.Weapons.Marker;
using Content.Shared._White.BackStab;
using Content.Shared.Damage;
using Content.Shared.Stunnable;

namespace Content.Server.Weapons;

public sealed class DamageMarkerSystem : SharedDamageMarkerSystem
{
    // Lavaland Change Start
    [Dependency] private readonly PressureEfficiencyChangeSystem _pressure = default!;
    [Dependency] private readonly BackStabSystem _backstab = default!;
    [Dependency] private readonly SharedStunSystem _stun = default!;
    [Dependency] private readonly DamageableSystem _damageable = default!;
    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<DamageMarkerComponent, ApplyMarkerBonusEvent>(OnApplyMarkerBonus);
    }

    private void OnApplyMarkerBonus(EntityUid uid, DamageMarkerComponent component, ref ApplyMarkerBonusEvent args)
    {
        if (!TryComp<DamageableComponent>(uid, out var damageable))
            return;

        if (TryComp<DamageBoostOnMarkerComponent>(args.Weapon, out var boost))
        {
            var pressureMultiplier = 65f;

            if (TryComp<PressureDamageChangeComponent>(args.Weapon, out var pressure)
                && _pressure.ApplyModifier((args.Weapon, pressure)))
                pressureMultiplier = pressure.AppliedModifier;

            if (boost.BackstabBoost != null
                && _backstab.TryBackstab(uid, args.User, Angle.FromDegrees(65d), playSound: false))
                _damageable.TryChangeDamage(uid,
                (boost.BackstabBoost + boost.Boost) * pressureMultiplier,
                damageable: damageable,
                origin: args.User);
            else
                _damageable.TryChangeDamage(uid,
                boost.Boost * pressureMultiplier,
                damageable: damageable,
                origin: args.User);
        }
    }
    // Lavaland Change End
}