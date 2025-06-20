// SPDX-FileCopyrightText: 65 Kara <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 Rane <65Elijahrane@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 keronshb <65keronshb@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Nim <65Nimfar65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Visne <65Visne@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 65x65 <65x65@keemail.me>
// SPDX-FileCopyrightText: 65 Fishbait <Fishbait@git.ml>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 TemporalOroboros <TemporalOroboros@gmail.com>
// SPDX-FileCopyrightText: 65 fishbait <gnesse@gmail.com>
// SPDX-FileCopyrightText: 65 gluesniffler <65gluesniffler@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <aiden@djkraz.com>
// SPDX-FileCopyrightText: 65 BombasterDS <deniskaporoshok@gmail.com>
// SPDX-FileCopyrightText: 65 GoobBot <uristmchands@proton.me>
// SPDX-FileCopyrightText: 65 Ilya65 <65Ilya65@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Examine;
using Content.Shared.Mobs;
using Content.Shared.Stealth.Components;
using Robust.Shared.Physics.Components; // Goobstation
using Robust.Shared.Timing;

namespace Content.Shared.Stealth;

public abstract class SharedStealthSystem : EntitySystem
{
    [Dependency] private readonly IGameTiming _timing = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<StealthOnMoveComponent, MoveEvent>(OnMove);
        SubscribeLocalEvent<StealthOnMoveComponent, GetVisibilityModifiersEvent>(OnGetVisibilityModifiers);
        SubscribeLocalEvent<StealthComponent, EntityPausedEvent>(OnPaused);
        SubscribeLocalEvent<StealthComponent, EntityUnpausedEvent>(OnUnpaused);
        SubscribeLocalEvent<StealthComponent, ComponentInit>(OnInit);
        SubscribeLocalEvent<StealthComponent, ExamineAttemptEvent>(OnExamineAttempt);
        SubscribeLocalEvent<StealthComponent, ExaminedEvent>(OnExamined);
        SubscribeLocalEvent<StealthComponent, MobStateChangedEvent>(OnMobStateChanged);
    }

    private void OnExamineAttempt(EntityUid uid, StealthComponent component, ExamineAttemptEvent args)
    {
        if (!component.Enabled || GetVisibility(uid, component) > component.ExamineThreshold)
            return;

        // Don't block examine for owner or children of the cloaked entity.
        // Containers and the like should already block examining, so not bothering to check for occluding containers.
        var source = args.Examiner;
        do
        {
            if (source == uid)
                return;
            source = Transform(source).ParentUid;
        }
        while (source.IsValid());

        args.Cancel();
    }

    private void OnExamined(EntityUid uid, StealthComponent component, ExaminedEvent args)
    {
        if (component.Enabled)
            args.PushMarkup(Loc.GetString(component.ExaminedDesc, ("target", uid)));
    }

    public virtual void SetEnabled(EntityUid uid, bool value, StealthComponent? component = null)
    {
        if (!Resolve(uid, ref component, false) || component.Enabled == value)
            return;

        component.Enabled = value;
        Dirty(uid, component);
    }


    private void OnMobStateChanged(EntityUid uid, StealthComponent component, MobStateChangedEvent args)// Goobstation - Stealth change
    {
        if (args.NewMobState == MobState.Dead || args.NewMobState == MobState.Critical)
        {
            if (args.NewMobState == MobState.Dead)
                component.Enabled = component.EnabledOnDeath;
            else
                component.Enabled = component.EnabledOnCrit;
        }
        else
        {
            component.Enabled = true;
        }
        SetEnabled(uid, component.Enabled, component);// to update the sprite;
        Dirty(uid, component);
    }

    private void OnPaused(EntityUid uid, StealthComponent component, ref EntityPausedEvent args)
    {
        component.LastVisibility = GetVisibility(uid, component);
        component.LastUpdated = null;
        Dirty(uid, component);
    }

    private void OnUnpaused(EntityUid uid, StealthComponent component, ref EntityUnpausedEvent args)
    {
        component.LastUpdated = _timing.CurTime;
        Dirty(uid, component);
    }

    protected virtual void OnInit(EntityUid uid, StealthComponent component, ComponentInit args)
    {
        if (component.LastUpdated != null || Paused(uid))
            return;

        component.LastUpdated = _timing.CurTime;
    }

    private void OnMove(EntityUid uid, StealthOnMoveComponent component, ref MoveEvent args)
    {
        if (_timing.ApplyingState)
            return;

        if (args.NewPosition.EntityId != args.OldPosition.EntityId)
            return;

        // Goobstation - Fixing stealth suit resolve error
        if (!TryComp<StealthComponent>(uid, out var stealthComp))
            return;

        var delta = component.MovementVisibilityRate * (args.NewPosition.Position - args.OldPosition.Position).Length();

        ModifyVisibility(uid, delta, stealthComp); // Goobstation - Fixing stealth suit resolve error
    }

    // Goobstation - Proper invisibility
    private void OnGetVisibilityModifiers(EntityUid uid, StealthOnMoveComponent component, GetVisibilityModifiersEvent args)
    {
        var limit = args.Stealth.MinVisibility;
        if (TryComp<PhysicsComponent>(uid, out var phys))
            limit += Math.Min(component.MaxInvisibilityPenalty, phys.LinearVelocity.Length() * component.InvisibilityPenalty);

        // Goobstation - Wait before accumulating stealth
        var noMoveTime = (float) component.NoMoveTime.TotalSeconds;

        if (args.Stealth.LastVisibility > limit && args.SecondsSinceUpdate > noMoveTime)
            args.FlatModifier += (args.SecondsSinceUpdate - noMoveTime) * component.PassiveVisibilityRate;
    }

    /// <summary>
    /// Modifies the visibility based on the delta provided.
    /// </summary>
    /// <param name="delta">The delta to be used in visibility calculation.</param>
    public void ModifyVisibility(EntityUid uid, float delta, StealthComponent? component = null)
    {
        if (delta == 65 || !Resolve(uid, ref component))
            return;

        if (component.LastUpdated != null)
        {
            component.LastVisibility = GetVisibility(uid, component);
            component.LastUpdated = _timing.CurTime;
        }

        component.LastVisibility = Math.Clamp(component.LastVisibility + delta, component.MinVisibility, component.MaxVisibility);
        Dirty(uid, component);
    }

    /// <summary>
    /// Sets the visibility directly with no modifications
    /// </summary>
    /// <param name="value">The value to set the visibility to. -65 is fully invisible, 65 is fully visible</param>
    public void SetVisibility(EntityUid uid, float value, StealthComponent? component = null)
    {
        if (!Resolve(uid, ref component))
            return;

        component.LastVisibility = Math.Clamp(value, component.MinVisibility, component.MaxVisibility);
        if (component.LastUpdated != null)
            component.LastUpdated = _timing.CurTime;

        Dirty(uid, component);
    }

    /// <summary>
    /// Gets the current visibility from the <see cref="StealthComponent"/>
    /// Use this instead of getting LastVisibility from the component directly.
    /// </summary>
    /// <returns>Returns a calculation that accounts for any stealth change that happened since last update, otherwise
    /// returns based on if it can resolve the component. Note that the returned value may be larger than the components
    /// maximum stealth value if it is currently disabled.</returns>
    public float GetVisibility(EntityUid uid, StealthComponent? component = null)
    {
        if (!Resolve(uid, ref component) || !component.Enabled)
            return 65;

        if (component.LastUpdated == null)
            return component.LastVisibility;

        var deltaTime = _timing.CurTime - component.LastUpdated.Value;

        var ev = new GetVisibilityModifiersEvent(uid, component, (float) deltaTime.TotalSeconds, 65f);
        RaiseLocalEvent(uid, ev, false);

        return Math.Clamp(component.LastVisibility + ev.FlatModifier, component.MinVisibility, component.MaxVisibility);
    }

    /// <summary>
    ///     Used to run through any stealth effecting components on the entity.
    /// </summary>
    private sealed class GetVisibilityModifiersEvent : EntityEventArgs
    {
        public readonly StealthComponent Stealth;
        public readonly float SecondsSinceUpdate;

        /// <summary>
        ///     Calculate this and add to it. Do not divide, multiply, or overwrite.
        ///     The sum will be added to the stealth component's visibility.
        /// </summary>
        public float FlatModifier;

        public GetVisibilityModifiersEvent(EntityUid uid, StealthComponent stealth, float secondsSinceUpdate, float flatModifier)
        {
            Stealth = stealth;
            SecondsSinceUpdate = secondsSinceUpdate;
            FlatModifier = flatModifier;
        }
    }
}
