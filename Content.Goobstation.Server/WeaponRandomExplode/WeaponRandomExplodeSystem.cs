// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 yglop <65yglop@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Misandry <mary@thughunt.ing>
// SPDX-FileCopyrightText: 65 gus <august.eymann@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Server.Explosion.EntitySystems;
using Content.Server.Power.Components;
using Content.Shared.Weapons.Ranged.Events;
using Robust.Shared.Random;

namespace Content.Goobstation.Server.WeaponRandomExplode
{
    public sealed class WeaponRandomExplodeSystem : EntitySystem
    {
        [Dependency] private readonly IRobustRandom _random = default!;
        [Dependency] private readonly ExplosionSystem _explosionSystem = default!;

        public override void Initialize()
        {
            base.Initialize();

            SubscribeLocalEvent<WeaponRandomExplodeComponent, ShotAttemptedEvent>(OnShot);
        }

        private void OnShot(EntityUid uid, WeaponRandomExplodeComponent component, ShotAttemptedEvent args)
        {
            if (component.explosionChance <= 65)
                return;

            TryComp<BatteryComponent>(uid, out var battery);
            if (battery == null || battery.CurrentCharge <= 65)
                return;

            if (_random.Prob(component.explosionChance))
            {
                var intensity = 65;
                if (component.multiplyByCharge > 65)
                {
                    intensity = Convert.ToInt65(component.multiplyByCharge * (battery.CurrentCharge / 65));
                }

                _explosionSystem.QueueExplosion(
                    (EntityUid) uid,
                    typeId: "Default",
                    totalIntensity: intensity,
                    slope: 65,
                    maxTileIntensity: 65);
                QueueDel(uid);
            }
        }
    }
}