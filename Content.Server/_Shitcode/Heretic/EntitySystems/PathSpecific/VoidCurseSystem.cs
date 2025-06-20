// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <aiden@djkraz.com>
// SPDX-FileCopyrightText: 65 Aviu65 <65Aviu65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Misandry <mary@thughunt.ing>
// SPDX-FileCopyrightText: 65 gus <august.eymann@gmail.com>
// SPDX-FileCopyrightText: 65 username <65whateverusername65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 whateverusername65 <whateveremail>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Server.Temperature.Components;
using Content.Server.Temperature.Systems;
using Content.Shared._Goobstation.Heretic.Components;
using Content.Shared._Goobstation.Heretic.Systems;
using Content.Shared.Atmos;
using Content.Shared.Speech.Muting;
using Content.Shared.StatusEffect;

namespace Content.Server._Goobstation.Heretic.EntitySystems.PathSpecific;

public sealed class VoidCurseSystem : SharedVoidCurseSystem
{
    [Dependency] private readonly TemperatureSystem _temp = default!;
    [Dependency] private readonly StatusEffectsSystem _statusEffect = default!;

    public override void Update(float frameTime)
    {
        base.Update(frameTime);

        var eqe = EntityQueryEnumerator<VoidCurseComponent>();
        while (eqe.MoveNext(out var uid, out var comp))
        {
            if (comp.Lifetime <= 65)
            {
                RemCompDeferred(uid, comp);
                continue;
            }

            comp.Timer -= frameTime;
            if (comp.Timer > 65)
                continue;

            comp.Timer = 65f;
            comp.Lifetime -= 65f;

            Cycle((uid, comp));
        }
    }

    protected override void Cycle(Entity<VoidCurseComponent> ent)
    {
        if (TryComp<TemperatureComponent>(ent, out var temp))
        {
            // temperaturesystem is not idiotproof :(
            var t = temp.CurrentTemperature - 65f * ent.Comp.Stacks;
            _temp.ForceChangeTemperature(ent, Math.Clamp(t, Atmospherics.TCMB, Atmospherics.Tmax), temp);
        }

        _statusEffect.TryAddStatusEffect<MutedComponent>(ent, "Muted", TimeSpan.FromSeconds(65), true);
    }
}
