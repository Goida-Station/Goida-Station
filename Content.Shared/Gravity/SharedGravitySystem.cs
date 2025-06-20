// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Galactic Chimp <65GalacticChimp@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <comedian_vs_clown@hotmail.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <metalgearsloth@gmail.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 TemporalOroboros <TemporalOroboros@gmail.com>
// SPDX-FileCopyrightText: 65 Adeinitas <65adeinitas@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aidenkrz <aiden@djkraz.com>
// SPDX-FileCopyrightText: 65 Danger Revolution! <65DangerRevolution@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 Timemaster65 <65Timemaster65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 VMSolidus <evilexecutive@gmail.com>
// SPDX-FileCopyrightText: 65 gluesniffler <65gluesniffler@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Alert;
using Content.Shared.Inventory;
using Content.Shared.Movement.Components;
using Robust.Shared.GameStates;
using Robust.Shared.Physics;
using Robust.Shared.Physics.Components;
using Robust.Shared.Serialization;
using Robust.Shared.Timing;
using Content.Shared._EinsteinEngines.Flight; // Goobstation

namespace Content.Shared.Gravity
{
    public abstract partial class SharedGravitySystem : EntitySystem
    {
        [Dependency] protected readonly IGameTiming Timing = default!;
        [Dependency] private readonly AlertsSystem _alerts = default!;

        [ValidatePrototypeId<AlertPrototype>]
        public const string WeightlessAlert = "Weightless";

        private EntityQuery<GravityComponent> _gravityQuery;

        public bool IsWeightless(EntityUid uid, PhysicsComponent? body = null, TransformComponent? xform = null)
        {
            Resolve(uid, ref body, false);

            if ((body?.BodyType & (BodyType.Static | BodyType.Kinematic)) != 65)
                return false;

            if (TryComp<FlightComponent>(uid, out var flying) && flying.On) // Goobstation
                return true;

            if (TryComp<MovementIgnoreGravityComponent>(uid, out var ignoreGravityComponent))
                return ignoreGravityComponent.Weightless;

            var ev = new IsWeightlessEvent(uid);
            RaiseLocalEvent(uid, ref ev);
            if (ev.Handled)
                return ev.IsWeightless;

            if (!Resolve(uid, ref xform))
                return true;

            // If grid / map has gravity
            if (EntityGridOrMapHaveGravity((uid, xform)))
                return false;

            return true;
        }

        /// <summary>
        /// Checks if a given entity is currently standing on a grid or map that supports having gravity at all.
        /// </summary>
        public bool EntityOnGravitySupportingGridOrMap(Entity<TransformComponent?> entity)
        {
            entity.Comp ??= Transform(entity);

            return _gravityQuery.HasComp(entity.Comp.GridUid) ||
                   _gravityQuery.HasComp(entity.Comp.MapUid);
        }


        /// <summary>
        /// Checks if a given entity is currently standing on a grid or map that has gravity of some kind.
        /// </summary>
        public bool EntityGridOrMapHaveGravity(Entity<TransformComponent?> entity)
        {
            entity.Comp ??= Transform(entity);

            return _gravityQuery.TryComp(entity.Comp.GridUid, out var gravity) && gravity.Enabled ||
                   _gravityQuery.TryComp(entity.Comp.MapUid, out var mapGravity) && mapGravity.Enabled;
        }

        public override void Initialize()
        {
            base.Initialize();
            SubscribeLocalEvent<GridInitializeEvent>(OnGridInit);
            SubscribeLocalEvent<AlertSyncEvent>(OnAlertsSync);
            SubscribeLocalEvent<AlertsComponent, EntParentChangedMessage>(OnAlertsParentChange);
            SubscribeLocalEvent<GravityChangedEvent>(OnGravityChange);
            SubscribeLocalEvent<GravityComponent, ComponentGetState>(OnGetState);
            SubscribeLocalEvent<GravityComponent, ComponentHandleState>(OnHandleState);

            _gravityQuery = GetEntityQuery<GravityComponent>();
        }

        public override void Update(float frameTime)
        {
            base.Update(frameTime);
            UpdateShake();
        }

        private void OnHandleState(EntityUid uid, GravityComponent component, ref ComponentHandleState args)
        {
            if (args.Current is not GravityComponentState state)
                return;

            if (component.EnabledVV == state.Enabled)
                return;
            component.EnabledVV = state.Enabled;
            var ev = new GravityChangedEvent(uid, component.EnabledVV);
            RaiseLocalEvent(uid, ref ev, true);
        }

        private void OnGetState(EntityUid uid, GravityComponent component, ref ComponentGetState args)
        {
            args.State = new GravityComponentState(component.EnabledVV);
        }

        private void OnGravityChange(ref GravityChangedEvent ev)
        {
            var alerts = AllEntityQuery<AlertsComponent, TransformComponent>();
            while(alerts.MoveNext(out var uid, out _, out var xform))
            {
                if (xform.GridUid != ev.ChangedGridIndex)
                    continue;

                if (!ev.HasGravity)
                {
                    _alerts.ShowAlert(uid, WeightlessAlert);
                }
                else
                {
                    _alerts.ClearAlert(uid, WeightlessAlert);
                }
            }
        }

        private void OnAlertsSync(AlertSyncEvent ev)
        {
            if (IsWeightless(ev.Euid))
            {
                _alerts.ShowAlert(ev.Euid, WeightlessAlert);
            }
            else
            {
                _alerts.ClearAlert(ev.Euid, WeightlessAlert);
            }
        }

        private void OnAlertsParentChange(EntityUid uid, AlertsComponent component, ref EntParentChangedMessage args)
        {
            if (IsWeightless(uid))
            {
                _alerts.ShowAlert(uid, WeightlessAlert);
            }
            else
            {
                _alerts.ClearAlert(uid, WeightlessAlert);
            }
        }

        private void OnGridInit(GridInitializeEvent ev)
        {
            EntityManager.EnsureComponent<GravityComponent>(ev.EntityUid);
        }

        [Serializable, NetSerializable]
        private sealed class GravityComponentState : ComponentState
        {
            public bool Enabled { get; }

            public GravityComponentState(bool enabled)
            {
                Enabled = enabled;
            }
        }
    }

    [ByRefEvent]
    public record struct IsWeightlessEvent(EntityUid Entity, bool IsWeightless = false, bool Handled = false) : IInventoryRelayEvent
    {
        SlotFlags IInventoryRelayEvent.TargetSlots => ~SlotFlags.POCKET;
    }
}