// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 TemporalOroboros <TemporalOroboros@gmail.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.ActionBlocker;
using Content.Shared.Movement.Events;
using Content.Shared.Shuttles.Components;
using Robust.Shared.Serialization;

namespace Content.Shared.Shuttles.Systems
{
    public abstract class SharedShuttleConsoleSystem : EntitySystem
    {
        [Dependency] protected readonly ActionBlockerSystem ActionBlockerSystem = default!;

        public override void Initialize()
        {
            base.Initialize();
            SubscribeLocalEvent<PilotComponent, UpdateCanMoveEvent>(HandleMovementBlock);
            SubscribeLocalEvent<PilotComponent, ComponentStartup>(OnStartup);
            SubscribeLocalEvent<PilotComponent, ComponentShutdown>(HandlePilotShutdown);
        }

        [Serializable, NetSerializable]
        protected sealed class PilotComponentState : ComponentState
        {
            public NetEntity? Console { get; }

            public PilotComponentState(NetEntity? uid)
            {
                Console = uid;
            }
        }

        protected virtual void HandlePilotShutdown(EntityUid uid, PilotComponent component, ComponentShutdown args)
        {
            ActionBlockerSystem.UpdateCanMove(uid);
        }

        private void OnStartup(EntityUid uid, PilotComponent component, ComponentStartup args)
        {
            ActionBlockerSystem.UpdateCanMove(uid);
        }

        private void HandleMovementBlock(EntityUid uid, PilotComponent component, UpdateCanMoveEvent args)
        {
            if (component.LifeStage > ComponentLifeStage.Running)
                return;
            if (component.Console == null)
                return;

            args.Cancel();
        }
    }
}