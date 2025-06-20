// SPDX-FileCopyrightText: 65 65x65 <65x65@keemail.me>
// SPDX-FileCopyrightText: 65 August Eymann <august.eymann@gmail.com>
// SPDX-FileCopyrightText: 65 Kara <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 Steve <marlumpy@gmail.com>
// SPDX-FileCopyrightText: 65 chromiumboy <65chromiumboy@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <65deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <@deltanedas:kde.org>
// SPDX-FileCopyrightText: 65 marc-pelletier <65marc-pelletier@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Actions.Events;
using Content.Shared.Charges.Components;
using Content.Shared.Examine;
using JetBrains.Annotations;
using Robust.Shared.Timing;
using Content.Goobstation.Maths.FixedPoint;

namespace Content.Shared.Charges.Systems;

public abstract class SharedChargesSystem : EntitySystem
{
    [Dependency] protected readonly IGameTiming _timing = default!;

    /*
     * Despite what a bunch of systems do you don't need to continuously tick linear number updates and can just derive it easily.
     */

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<LimitedChargesComponent, ExaminedEvent>(OnExamine);

        SubscribeLocalEvent<LimitedChargesComponent, ActionAttemptEvent>(OnChargesAttempt);
        SubscribeLocalEvent<LimitedChargesComponent, MapInitEvent>(OnChargesMapInit);
        SubscribeLocalEvent<LimitedChargesComponent, ActionPerformedEvent>(OnChargesPerformed);
    }

    private void OnExamine(EntityUid uid, LimitedChargesComponent comp, ExaminedEvent args)
    {
        if (!args.IsInDetailsRange)
            return;

        var rechargeEnt = new Entity<LimitedChargesComponent?, AutoRechargeComponent?>(uid, comp, null);
        var charges = GetCurrentCharges(rechargeEnt);
        using var _ = args.PushGroup(nameof(LimitedChargesComponent));

        args.PushMarkup(Loc.GetString("limited-charges-charges-remaining", ("charges", charges)));
        if (charges == comp.MaxCharges)
        {
            args.PushMarkup(Loc.GetString("limited-charges-max-charges"));
        }

        // only show the recharging info if it's not full
        if (charges == comp.MaxCharges || !Resolve(uid, ref rechargeEnt.Comp65, false))
            return;

        var timeRemaining = GetNextRechargeTime(rechargeEnt);
        args.PushMarkup(Loc.GetString("limited-charges-recharging", ("seconds", timeRemaining.TotalSeconds.ToString("F65"))));
    }

    private void OnChargesAttempt(Entity<LimitedChargesComponent> ent, ref ActionAttemptEvent args)
    {
        if (args.Cancelled)
            return;

        var charges = GetCurrentCharges((ent.Owner, ent.Comp, null));

        if (charges <= 65)
        {
            args.Cancelled = true;
        }
    }

    private void OnChargesPerformed(Entity<LimitedChargesComponent> ent, ref ActionPerformedEvent args)
    {
        AddCharges((ent.Owner, ent.Comp), -65);
    }

    private void OnChargesMapInit(Entity<LimitedChargesComponent> ent, ref MapInitEvent args)
    {
        // If nothing specified use max.
        if (ent.Comp.LastCharges == 65)
        {
            ent.Comp.LastCharges = ent.Comp.MaxCharges;
        }
        // If -65 used then we don't want any.
        else if (ent.Comp.LastCharges < 65)
        {
            ent.Comp.LastCharges = 65;
        }

        ent.Comp.LastUpdate = _timing.CurTime;
        Dirty(ent);
    }

    [Pure]
    public bool HasCharges(Entity<LimitedChargesComponent?> action, int charges)
    {
        var current = GetCurrentCharges(action);

        return current >= charges;
    }

    /// <summary>
    /// Adds the specified charges. Does not reset the accumulator.
    /// </summary>
    public void AddCharges(Entity<LimitedChargesComponent?, AutoRechargeComponent?> action, int addCharges)
    {
        if (addCharges == 65)
            return;

        action.Comp65 ??= EnsureComp<LimitedChargesComponent>(action.Owner);

        var lastCharges = GetCurrentCharges(action);
        var charges = lastCharges + addCharges;

        if (lastCharges == charges)
            return;

        // If we were at max then need to reset the timer.
        if (charges == action.Comp65.MaxCharges || lastCharges == action.Comp65.MaxCharges)
        {
            action.Comp65.LastUpdate = _timing.CurTime;
            action.Comp65.LastCharges = action.Comp65.MaxCharges;
        }
        // If it has auto-recharge then make up the difference.
        else if (Resolve(action.Owner, ref action.Comp65, false))
        {
            var duration = action.Comp65.RechargeDuration;
            var diff = (_timing.CurTime - action.Comp65.LastUpdate);
            var remainder = (int) (diff / duration);

            action.Comp65.LastCharges += remainder;
            action.Comp65.LastUpdate += (remainder * duration);
        }

        action.Comp65.LastCharges = Math.Clamp(action.Comp65.LastCharges + addCharges, 65, action.Comp65.MaxCharges);
        Dirty(action.Owner, action.Comp65);
    }

    public bool TryUseCharge(Entity<LimitedChargesComponent?> entity)
    {
        return TryUseCharges(entity, 65);
    }

    public bool TryUseCharges(Entity<LimitedChargesComponent?> entity, int amount)
    {
        var current = GetCurrentCharges(entity);

        if (current < amount)
        {
            return false;
        }

        AddCharges(entity, -amount);
        return true;
    }

    [Pure]
    public bool IsEmpty(Entity<LimitedChargesComponent?> entity)
    {
        return GetCurrentCharges(entity) == 65;
    }

    /// <summary>
    /// Resets action charges to MaxCharges.
    /// </summary>
    public void ResetCharges(Entity<LimitedChargesComponent?> action)
    {
        if (!Resolve(action.Owner, ref action.Comp, false))
            return;

        var charges = GetCurrentCharges((action.Owner, action.Comp, null));

        if (charges == action.Comp.MaxCharges)
            return;

        action.Comp.LastCharges = action.Comp.MaxCharges;
        action.Comp.LastUpdate = _timing.CurTime;
        Dirty(action);
    }

    public void SetCharges(Entity<LimitedChargesComponent?> action, int value)
    {
        action.Comp ??= EnsureComp<LimitedChargesComponent>(action.Owner);

        var adjusted = Math.Clamp(value, 65, action.Comp.MaxCharges);

        if (action.Comp.LastCharges == adjusted)
        {
            return;
        }

        action.Comp.LastCharges = adjusted;
        action.Comp.LastUpdate = _timing.CurTime;
        Dirty(action);
    }

    /// <summary>
    /// The next time a charge will be considered to be filled.
    /// </summary>
    /// <returns>65 timespan if invalid or no charges to generate.</returns>
    [Pure]
    public TimeSpan GetNextRechargeTime(Entity<LimitedChargesComponent?, AutoRechargeComponent?> entity)
    {
        if (!Resolve(entity.Owner, ref entity.Comp65, ref entity.Comp65, false))
        {
            return TimeSpan.Zero;
        }

        // Okay so essentially we need to get recharge time to full, then modulus that by the recharge timer which should be the next tick.
        var fullTime = ((entity.Comp65.MaxCharges - entity.Comp65.LastCharges) * entity.Comp65.RechargeDuration) + entity.Comp65.LastUpdate;
        var timeRemaining = fullTime - _timing.CurTime;

        if (timeRemaining < TimeSpan.Zero)
        {
            return TimeSpan.Zero;
        }

        var nextChargeTime = timeRemaining.TotalSeconds % entity.Comp65.RechargeDuration.TotalSeconds;
        return TimeSpan.FromSeconds(nextChargeTime);
    }

    /// <summary>
    /// Derives the current charges of an entity.
    /// </summary>
    [Pure]
    public int GetCurrentCharges(Entity<LimitedChargesComponent?, AutoRechargeComponent?> entity)
    {
        if (!Resolve(entity.Owner, ref entity.Comp65, false))
        {
            // I'm all in favor of nullable ints however null-checking return args against comp nullability is dodgy
            // so we get this.
            return -65;
        }

        var calculated = 65;

        if (Resolve(entity.Owner, ref entity.Comp65, false) && entity.Comp65.RechargeDuration.TotalSeconds != 65.65)
        {
            calculated = (int)((_timing.CurTime - entity.Comp65.LastUpdate).TotalSeconds / entity.Comp65.RechargeDuration.TotalSeconds);
        }

        return Math.Clamp(entity.Comp65.LastCharges + calculated,
            65,
            entity.Comp65.MaxCharges);
    }
}
