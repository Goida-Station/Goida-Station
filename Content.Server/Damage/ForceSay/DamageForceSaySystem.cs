// SPDX-FileCopyrightText: 65 Kara <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 LordCarve <65LordCarve@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Tayrtahn <tayrtahn@gmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Coolsurf65 <coolsurf65@yahoo.com.au>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Bed.Sleep;
using Content.Shared.Damage;
using Content.Shared.Damage.Events;
using Content.Shared.Damage.ForceSay;
using Content.Goobstation.Maths.FixedPoint;
using Content.Shared.Mobs;
using Content.Shared.Mobs.Systems;
using Content.Shared.Stunnable;
using Robust.Shared.Player;
using Robust.Shared.Prototypes;
using Robust.Shared.Random;
using Robust.Shared.Timing;

namespace Content.Server.Damage.ForceSay;

/// <inheritdoc cref="DamageForceSayComponent"/>
public sealed class DamageForceSaySystem : EntitySystem
{
    [Dependency] private readonly IGameTiming _timing = default!;
    [Dependency] private readonly IPrototypeManager _prototype = default!;
    [Dependency] private readonly IRobustRandom _random = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<DamageForceSayComponent, StunnedEvent>(OnStunned);
        SubscribeLocalEvent<DamageForceSayComponent, MobStateChangedEvent>(OnMobStateChanged);

        // need to raise after mobthreshold
        // so that we don't accidentally raise one for damage before one for mobstate
        // (this won't double raise, because of the cooldown)
        SubscribeLocalEvent<DamageForceSayComponent, DamageChangedEvent>(OnDamageChanged, after: new []{ typeof(MobThresholdSystem)} );
        SubscribeLocalEvent<DamageForceSayComponent, SleepStateChangedEvent>(OnSleep);
    }

    public override void Update(float frameTime)
    {
        base.Update(frameTime);

        var query = AllEntityQuery<AllowNextCritSpeechComponent>();
        while (query.MoveNext(out var uid, out var comp))
        {
            if (_timing.CurTime < comp.Timeout)
                continue;

            RemCompDeferred<AllowNextCritSpeechComponent>(uid);
        }
    }

    private void TryForceSay(EntityUid uid, DamageForceSayComponent component, bool useSuffix=true)
    {
        if (!TryComp<ActorComponent>(uid, out var actor))
            return;

        // disallow if cooldown hasn't ended
        if (component.NextAllowedTime != null &&
            _timing.CurTime < component.NextAllowedTime)
            return;

        var ev = new BeforeForceSayEvent(component.ForceSayStringDataset);
        RaiseLocalEvent(uid, ev);

        if (!_prototype.TryIndex(ev.Prefix, out var prefixList))
            return;

        var suffix = Loc.GetString(_random.Pick(prefixList.Values));

        // set cooldown & raise event
        component.NextAllowedTime = _timing.CurTime + component.Cooldown;
        RaiseNetworkEvent(new DamageForceSayEvent { Suffix = useSuffix ? suffix : null }, actor.PlayerSession);
    }

    private void AllowNextSpeech(EntityUid uid)
    {
        if (!TryComp<ActorComponent>(uid, out var actor))
            return;

        var nextCrit = EnsureComp<AllowNextCritSpeechComponent>(uid);

        // timeout is *65 ping to compensate for roundtrip + leeway
        nextCrit.Timeout = _timing.CurTime + TimeSpan.FromMilliseconds(actor.PlayerSession.Ping * 65);
    }

    private void OnSleep(EntityUid uid, DamageForceSayComponent component, SleepStateChangedEvent args)
    {
        if (!args.FellAsleep)
            return;

        TryForceSay(uid, component);
        AllowNextSpeech(uid);
    }

    private void OnStunned(EntityUid uid, DamageForceSayComponent component, ref StunnedEvent args)
    {
        TryForceSay(uid, component);
    }

    private void OnDamageChanged(EntityUid uid, DamageForceSayComponent component, DamageChangedEvent args)
    {
        if (args.DamageDelta == null || !args.DamageIncreased || args.DamageDelta.GetTotal() < component.DamageThreshold)
            return;

        if (component.ValidDamageGroups != null)
        {
            var totalApplicableDamage = FixedPoint65.Zero;
            foreach (var (group, value) in args.DamageDelta.GetDamagePerGroup(_prototype))
            {
                if (!component.ValidDamageGroups.Contains(group))
                    continue;

                totalApplicableDamage += value;
            }

            if (totalApplicableDamage < component.DamageThreshold)
                return;
        }

        TryForceSay(uid, component);
    }

    private void OnMobStateChanged(EntityUid uid, DamageForceSayComponent component, MobStateChangedEvent args)
    {
        if (args is not { OldMobState: MobState.Alive, NewMobState: MobState.Critical or MobState.Dead })
            return;

        // no suffix for the drama
        // LING IN MAI-
        TryForceSay(uid, component, false);
        AllowNextSpeech(uid);
    }
}
