// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <65Zumorica@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <gradientvera@outlook.com>
// SPDX-FileCopyrightText: 65 pointer-to-null <65pointer-to-null@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Emisse <65Emisse@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Kara <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 ShadowCommander <65ShadowCommander@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Ygg65 <y.laughing.man.y@gmail.com>
// SPDX-FileCopyrightText: 65 hubismal <65hubismal@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <metalgearsloth@gmail.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers@gmail.com>
// SPDX-FileCopyrightText: 65 Slava65 <65Slava65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Arendian <65Arendian@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Errant <65Errant-65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 KISS <65YuriyKiss@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 SlamBamActionman <65SlamBamActionman@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Yurii Kis <yurii.kis@smartteksas.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <comedian_vs_clown@hotmail.com>
// SPDX-FileCopyrightText: 65 username <65whateverusername65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 whateverusername65 <whateveremail>
// SPDX-FileCopyrightText: 65 Джексон Миссиссиппи <tripwiregamer@gmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aidenkrz <aiden@djkraz.com>
// SPDX-FileCopyrightText: 65 Aviu65 <65Aviu65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Ilya65 <65Ilya65@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Administration.Logs;
using Content.Shared.Database;
using Content.Shared.Inventory;
using Content.Shared.Movement.Components;
using Content.Shared.Movement.Systems;
using Content.Shared.StatusEffect;
using Content.Shared.StepTrigger.Systems;
using Content.Shared.Stunnable;
using Content.Shared.Throwing;
using JetBrains.Annotations;
using Robust.Shared.Audio.Systems;
using Robust.Shared.Containers;
using Robust.Shared.Network;
using Robust.Shared.Physics.Components;
using Robust.Shared.Physics.Events;
using Robust.Shared.Physics.Systems;
using Robust.Shared.Utility;

namespace Content.Shared.Slippery;

[UsedImplicitly]
public sealed class SlipperySystem : EntitySystem
{
    [Dependency] private readonly INetManager _net = default!; // Goobstation
    [Dependency] private readonly ISharedAdminLogManager _adminLogger = default!;
    [Dependency] private readonly SharedAudioSystem _audio = default!;
    [Dependency] private readonly SharedStunSystem _stun = default!;
    [Dependency] private readonly StatusEffectsSystem _statusEffects = default!;
    [Dependency] private readonly SharedContainerSystem _container = default!;
    [Dependency] private readonly SharedPhysicsSystem _physics = default!;
    [Dependency] private readonly SpeedModifierContactsSystem _speedModifier = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<SlipperyComponent, StepTriggerAttemptEvent>(HandleAttemptCollide);
        SubscribeLocalEvent<SlipperyComponent, StepTriggeredOffEvent>(HandleStepTrigger);
        SubscribeLocalEvent<NoSlipComponent, SlipAttemptEvent>(OnNoSlipAttempt);
        SubscribeLocalEvent<SlowedOverSlipperyComponent, SlipAttemptEvent>(OnSlowedOverSlipAttempt);
        SubscribeLocalEvent<ThrownItemComponent, SlipCausingAttemptEvent>(OnThrownSlipAttempt);
        SubscribeLocalEvent<NoSlipComponent, InventoryRelayedEvent<SlipAttemptEvent>>((e, c, ev) => OnNoSlipAttempt(e, c, ev.Args));
        SubscribeLocalEvent<SlowedOverSlipperyComponent, InventoryRelayedEvent<SlipAttemptEvent>>((e, c, ev) => OnSlowedOverSlipAttempt(e, c, ev.Args));
        SubscribeLocalEvent<SlowedOverSlipperyComponent, InventoryRelayedEvent<GetSlowedOverSlipperyModifierEvent>>(OnGetSlowedOverSlipperyModifier);
        SubscribeLocalEvent<SlipperyComponent, EndCollideEvent>(OnEntityExit);
    }

    private void HandleStepTrigger(EntityUid uid, SlipperyComponent component, ref StepTriggeredOffEvent args)
    {
        TrySlip(uid, component, args.Tripper);
    }

    private void HandleAttemptCollide(
        EntityUid uid,
        SlipperyComponent component,
        ref StepTriggerAttemptEvent args)
    {
        args.Continue |= component.SlipData.SlipOnStep && CanSlip(uid, args.Tripper); // Goob edit
    }

    private static void OnNoSlipAttempt(EntityUid uid, NoSlipComponent component, SlipAttemptEvent args)
    {
        args.NoSlip = true;
    }

    private void OnSlowedOverSlipAttempt(EntityUid uid, SlowedOverSlipperyComponent component, SlipAttemptEvent args)
    {
        args.SlowOverSlippery = true;
    }

    private void OnThrownSlipAttempt(EntityUid uid, ThrownItemComponent comp, ref SlipCausingAttemptEvent args)
    {
        args.Cancelled = true;
    }

    private void OnGetSlowedOverSlipperyModifier(EntityUid uid, SlowedOverSlipperyComponent comp, ref InventoryRelayedEvent<GetSlowedOverSlipperyModifierEvent> args)
    {
        args.Args.SlowdownModifier *= comp.SlowdownModifier;
    }

    private void OnEntityExit(EntityUid uid, SlipperyComponent component, ref EndCollideEvent args)
    {
        if (HasComp<SpeedModifiedByContactComponent>(args.OtherEntity))
            _speedModifier.AddModifiedEntity(args.OtherEntity);
    }

    public bool CanSlip(EntityUid uid, EntityUid toSlip) // Goob edit
    {
        return !_container.IsEntityInContainer(uid)
                && _statusEffects.CanApplyEffect(toSlip, "KnockedDown", raiseEvent: false); // Goob edit
    }

    public void TrySlip(EntityUid uid, SlipperyComponent component, EntityUid other, bool requiresContact = true, bool force = false, bool predicted = true) // Goob edit
    {
        // Goob edit start
        if (!predicted && _net.IsClient)
            return;

        if ((HasComp<KnockedDownComponent>(other) || HasComp<StunnedComponent>(other)) && !component.SlipData.SuperSlippery)
            return;

        if (!force)
        {
            var attemptEv = new SlipAttemptEvent();
            RaiseLocalEvent(other, attemptEv);
            if (attemptEv.SlowOverSlippery)
                _speedModifier.AddModifiedEntity(other);

            if (attemptEv.NoSlip)
                return;

            var attemptCausingEv = new SlipCausingAttemptEvent();
            RaiseLocalEvent(uid, ref attemptCausingEv);
            if (attemptCausingEv.Cancelled)
                return;
        }

        var hardStun = component.SlipData.SuperSlippery; // Goobstation
        // Goob edit end

        var ev = new SlipEvent(other);
        RaiseLocalEvent(uid, ref ev);

        if (TryComp(other, out PhysicsComponent? physics) && !HasComp<SlidingComponent>(other))
        {
            _physics.SetLinearVelocity(other, physics.LinearVelocity * component.SlipData.LaunchForwardsMultiplier, body: physics);

            if (component.SlipData.SuperSlippery && requiresContact)
            {
                var sliding = EnsureComp<SlidingComponent>(other);
                sliding.CollidingEntities.Add(uid);
                DebugTools.Assert(_physics.GetContactingEntities(other, physics).Contains(uid));
            }
        }

        var playSound = !_statusEffects.HasStatusEffect(other, "KnockedDown");

        // goob edit - stunmeta
        var time = component.SlipData.ParalyzeTime;
        if (hardStun)
            _stun.TryParalyze(other, time, true);
        else
            _stun.KnockdownOrStun(other, time, true);

        // Preventing from playing the slip sound when you are already knocked down.
        if (playSound)
        {
            if (predicted)
                _audio.PlayPredicted(component.SlipSound, other, other);
            else
                _audio.PlayPvs(component.SlipSound, other);
        }

        _adminLogger.Add(LogType.Slip, LogImpact.Low,
            $"{ToPrettyString(other):mob} slipped on collision with {ToPrettyString(uid):entity}");
    }
}

/// <summary>
///     Raised on an entity to determine if it can slip or not.
/// </summary>
public sealed class SlipAttemptEvent : EntityEventArgs, IInventoryRelayEvent
{
    public bool NoSlip;

    public bool SlowOverSlippery;

    public SlotFlags TargetSlots { get; } = SlotFlags.FEET;
}

/// <summary>
/// Raised on an entity that is causing the slip event (e.g, the banana peel), to determine if the slip attempt should be cancelled.
/// </summary>
/// <param name="Cancelled">If the slip should be cancelled</param>
[ByRefEvent]
public record struct SlipCausingAttemptEvent (bool Cancelled);

/// Raised on an entity that CAUSED some other entity to slip (e.g., the banana peel).
/// <param name="Slipped">The entity being slipped</param>
[ByRefEvent]
public readonly record struct SlipEvent(EntityUid Slipped);
