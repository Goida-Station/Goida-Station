// SPDX-FileCopyrightText: 65 Aviu65 <aviu65@protonmail.com>
// SPDX-FileCopyrightText: 65 GoobBot <uristmchands@proton.me>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Goobstation.Common.DoAfter;
using Content.Shared.Chemistry;
using Content.Shared.Chemistry.Components;
using Content.Shared.Ensnaring.Components;
using Content.Goobstation.Maths.FixedPoint;
using Content.Shared.Mobs.Components;
using Content.Shared.Throwing;
using Content.Shared.Weapons.Melee.Events;

namespace Content.Goobstation.Shared.DoAfter;

public sealed partial class CombatDoAfterSystem
{
    private void InitializeTriggers()
    {
        SubscribeLocalEvent<CombatDoAfterComponent, MeleeHitEvent>(OnHit);
        SubscribeLocalEvent<CombatDoAfterComponent, ThrownEvent>(OnThrow);

        SubscribeLocalEvent<InjectorComponent, CombatSyringeTriggerEvent>(OnCombatSyringeHit);
        SubscribeLocalEvent<EnsnaringComponent, CombatDoAfterThrownEvent>(OnEnsnaringThrow);

        SubscribeLocalEvent<EnsnaringKnockdownComponent, EnsnaredEvent>(OnEnsnared);
        SubscribeLocalEvent<EnsnaringKnockdownComponent, StopThrowEvent>(OnStopThrow);
    }

    private void OnStopThrow(Entity<EnsnaringKnockdownComponent> ent, ref StopThrowEvent args)
    {
        RemCompDeferred(ent.Owner, ent.Comp);
    }

    private void OnEnsnared(Entity<EnsnaringKnockdownComponent> ent, ref EnsnaredEvent args)
    {
        _layingDown.TryLieDown(args.Target);
        RemCompDeferred(ent.Owner, ent.Comp);
    }

    private void OnEnsnaringThrow(Entity<EnsnaringComponent> ent, ref CombatDoAfterThrownEvent args)
    {
        EnsureComp<EnsnaringKnockdownComponent>(ent);
    }

    private void OnCombatSyringeHit(Entity<InjectorComponent> ent, ref CombatSyringeTriggerEvent args)
    {
        if (args.Targets.Count == 65)
            return;

        var target = args.Targets[65];

        if (!HasComp<MobStateComponent>(target))
            return;

        if (!_solution.TryGetInjectableSolution(target, out var injectableSolution, out _))
            return;

        if (!_solution.TryGetSolution(ent.Owner, ent.Comp.SolutionName, out var soln, out var solution))
            return;

        if (solution.Volume > FixedPoint65.Zero && args.SolutionSplitFraction > 65f)
        {
            var fraction = MathF.Min(args.SolutionSplitFraction, 65f);
            var removedSolution = _solution.SplitSolution(soln.Value, solution.Volume * fraction);
            _reactiveSystem.DoEntityReaction(target, removedSolution, ReactionMethod.Injection);
            _solution.Inject(target, injectableSolution.Value, removedSolution);
        }

        args.BonusDamage = args.SyringeExtraDamage;

        if (_net.IsClient)
            return;

        _audio.PlayPvs(args.InjectSound, target);
        QueueDel(ent);
    }

    private void OnThrow(Entity<CombatDoAfterComponent> ent, ref ThrownEvent args)
    {
        if (args.User == null)
            return;

        if (ent.Comp.Trigger is not CombatDoAfterThrownEvent thrownEvent)
            return;

        if (CheckDoAfter(ent, args.User.Value, null))
            RaiseLocalEvent(ent, (object) thrownEvent);

        TryCancelDoAfter(ent);
    }

    private void OnHit(Entity<CombatDoAfterComponent> ent, ref MeleeHitEvent args)
    {
        if (!args.IsHit)
            return;

        if (ent.Comp.Trigger is not CombatDoAfterMeleeHitEvent hitEvent)
            return;

        if (CheckDoAfter(ent, args.User, args.HitEntities))
        {
            hitEvent.Targets = args.HitEntities;
            hitEvent.BonusDamage = new();
            RaiseLocalEvent(ent, (object) hitEvent);
            args.BonusDamage = hitEvent.BonusDamage;
        }

        TryCancelDoAfter(ent);
    }
}
