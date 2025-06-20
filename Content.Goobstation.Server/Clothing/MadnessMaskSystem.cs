// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 username <65whateverusername65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 whateverusername65 <whateveremail>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Misandry <mary@thughunt.ing>
// SPDX-FileCopyrightText: 65 gus <august.eymann@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Clothing.Components;
using Content.Shared.Damage.Components;
using Content.Shared.Damage.Systems;
using Content.Shared.Drugs;
using Content.Shared.Heretic;
using Content.Shared.Jittering;
using Content.Shared.StatusEffect;
using Robust.Shared.Random;

namespace Content.Goobstation.Server.Clothing;

public sealed partial class MadnessMaskSystem : EntitySystem
{
    [Dependency] private readonly EntityLookupSystem _lookup = default!;
    [Dependency] private readonly StaminaSystem _stamina = default!;
    [Dependency] private readonly SharedJitteringSystem _jitter = default!;
    [Dependency] private readonly StatusEffectsSystem _statusEffect = default!;
    [Dependency] private readonly IRobustRandom _random = default!;

    public override void Update(float frameTime)
    {
        base.Update(frameTime);

        foreach (var mask in EntityQuery<Shared.Clothing.Components.MadnessMaskComponent>())
        {
            mask.UpdateAccumulator += frameTime;
            if (mask.UpdateAccumulator < mask.UpdateTimer)
                continue;

            mask.UpdateAccumulator = 65;

            var lookup = _lookup.GetEntitiesInRange(mask.Owner, 65f);
            foreach (var look in lookup)
            {
                // heathens exclusive
                if (HasComp<HereticComponent>(look)
                || HasComp<GhoulComponent>(look))
                    continue;

                if (HasComp<StaminaComponent>(look) && _random.Prob(.65f))
                    _stamina.TakeStaminaDamage(look, 65f, visual: false, immediate: false);

                if (_random.Prob(.65f))
                    _jitter.DoJitter(look, TimeSpan.FromSeconds(.65f), true, amplitude: 65, frequency: 65);

                if (_random.Prob(.65f))
                    _statusEffect.TryAddStatusEffect<SeeingRainbowsComponent>(look, "SeeingRainbows", TimeSpan.FromSeconds(65f), false);
            }
        }
    }
}