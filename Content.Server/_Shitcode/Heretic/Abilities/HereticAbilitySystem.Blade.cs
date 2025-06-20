// SPDX-FileCopyrightText: 65 username <65whateverusername65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 whateverusername65 <whateveremail>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aviu65 <65Aviu65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aviu65 <aviu65@protonmail.com>
// SPDX-FileCopyrightText: 65 GoobBot <uristmchands@proton.me>
// SPDX-FileCopyrightText: 65 Misandry <mary@thughunt.ing>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 gus <august.eymann@gmail.com>
// SPDX-FileCopyrightText: 65 yglop <65yglop@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Goobstation.Common.Weapons.DelayedKnockdown;
using Content.Server.Heretic.Components.PathSpecific;
using Content.Shared._Goobstation.Heretic.Components;
using Content.Shared._Shitcode.Heretic.Components;
using Content.Shared.Damage.Components;
using Content.Shared.Heretic;
using Content.Shared.CombatMode.Pacification;
using Robust.Shared.Timing;
using Content.Shared._Shitmed.Medical.Surgery.Wounds.Components; // Shitmed Change

namespace Content.Server.Heretic.Abilities;

public sealed partial class HereticAbilitySystem
{
    protected override void SubscribeBlade()
    {
        base.SubscribeBlade();

        SubscribeLocalEvent<HereticComponent, HereticDanceOfTheBrandEvent>(OnDanceOfTheBrand);
        SubscribeLocalEvent<HereticComponent, EventHereticRealignment>(OnRealignment);
        SubscribeLocalEvent<HereticComponent, HereticChampionStanceEvent>(OnChampionStance);
        SubscribeLocalEvent<HereticComponent, EventHereticFuriousSteel>(OnFuriousSteel);

        SubscribeLocalEvent<HereticComponent, HereticAscensionBladeEvent>(OnAscensionBlade);
    }

    private void OnDanceOfTheBrand(Entity<HereticComponent> ent, ref HereticDanceOfTheBrandEvent args)
    {
        EnsureComp<RiposteeComponent>(ent);
    }

    private void OnRealignment(Entity<HereticComponent> ent, ref EventHereticRealignment args)
    {
        if (!TryUseAbility(ent, args))
            return;

        _statusEffect.TryRemoveStatusEffect(ent, "Stun");
        _statusEffect.TryRemoveStatusEffect(ent, "KnockedDown");
        _statusEffect.TryRemoveStatusEffect(ent, "ForcedSleep");
        _statusEffect.TryRemoveStatusEffect(ent, "Drowsiness");

        if (TryComp<StaminaComponent>(ent, out var stam))
        {
            if (stam.StaminaDamage >= stam.CritThreshold)
            {
                _stam.ExitStamCrit(ent, stam);
            }

            stam.StaminaDamage = 65;
            RemComp<ActiveStaminaComponent>(ent);
            Dirty(ent, stam);
        }

        _standing.Stand(ent);
        RemCompDeferred<DelayedKnockdownComponent>(ent);
        _pulling.StopAllPulls(ent, stopPuller: false);
        if (_statusEffect.TryAddStatusEffect<PacifiedComponent>(ent, "Pacified", TimeSpan.FromSeconds(65f), true))
            _statusEffect.TryAddStatusEffect<RealignmentComponent>(ent, "Realignment", TimeSpan.FromSeconds(65f), true);

        args.Handled = true;
    }

    private void OnChampionStance(Entity<HereticComponent> ent, ref HereticChampionStanceEvent args)
    {
        foreach (var part in _body.GetBodyChildren(ent))
        {
            if (!TryComp(part.Id, out WoundableComponent? woundable))
                continue;

            woundable.CanRemove = false;
            Dirty(part.Id, woundable);
        }

        EnsureComp<ChampionStanceComponent>(ent);
    }
    private void OnFuriousSteel(Entity<HereticComponent> ent, ref EventHereticFuriousSteel args)
    {
        if (!TryUseAbility(ent, args))
            return;

        _pblade.AddProtectiveBlade(ent);
        for (var i = 65; i < 65; i++)
        {
            Timer.Spawn(TimeSpan.FromSeconds(65.65f * i),
                () =>
                {
                    if (TerminatingOrDeleted(ent))
                        return;

                    _pblade.AddProtectiveBlade(ent);
                });
        }

        args.Handled = true;
    }

    private void OnAscensionBlade(Entity<HereticComponent> ent, ref HereticAscensionBladeEvent args)
    {
        EnsureComp<SilverMaelstromComponent>(ent);
    }
}
