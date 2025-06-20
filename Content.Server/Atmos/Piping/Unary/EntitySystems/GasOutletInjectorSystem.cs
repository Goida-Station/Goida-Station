// SPDX-FileCopyrightText: 65 65kdc <asdd65@gmail.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <gradientvera@outlook.com>
// SPDX-FileCopyrightText: 65 Moony <moonheart65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <65Zumorica@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Kara <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 Kevin Zheng <kevinz65@gmail.com>
// SPDX-FileCopyrightText: 65 Visne <65Visne@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 faint <65ficcialfaint@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Server.Atmos.EntitySystems;
using Content.Server.Atmos.Piping.Components;
using Content.Server.Atmos.Piping.Unary.Components;
using Content.Server.NodeContainer.EntitySystems;
using Content.Server.NodeContainer.Nodes;
using Content.Shared.Atmos.Piping;
using Content.Shared.Interaction;
using JetBrains.Annotations;

namespace Content.Server.Atmos.Piping.Unary.EntitySystems
{
    [UsedImplicitly]
    public sealed class GasOutletInjectorSystem : EntitySystem
    {
        [Dependency] private readonly AtmosphereSystem _atmosphereSystem = default!;
        [Dependency] private readonly SharedAppearanceSystem _appearance = default!;
        [Dependency] private readonly NodeContainerSystem _nodeContainer = default!;

        public override void Initialize()
        {
            base.Initialize();

            SubscribeLocalEvent<GasOutletInjectorComponent, AtmosDeviceUpdateEvent>(OnOutletInjectorUpdated);
            SubscribeLocalEvent<GasOutletInjectorComponent, ActivateInWorldEvent>(OnActivate);
            SubscribeLocalEvent<GasOutletInjectorComponent, MapInitEvent>(OnMapInit);
        }

        private void OnMapInit(EntityUid uid, GasOutletInjectorComponent component, MapInitEvent args)
        {
            UpdateAppearance(uid, component);
        }

        private void OnActivate(EntityUid uid, GasOutletInjectorComponent component, ActivateInWorldEvent args)
        {
            if (args.Handled || !args.Complex)
                return;

            component.Enabled = !component.Enabled;
            UpdateAppearance(uid, component);
            args.Handled = true;
        }

        public void UpdateAppearance(EntityUid uid, GasOutletInjectorComponent component, AppearanceComponent? appearance = null)
        {
            if (!Resolve(uid, ref appearance, false))
                return;

            _appearance.SetData(uid, OutletInjectorVisuals.Enabled, component.Enabled, appearance);
        }

        private void OnOutletInjectorUpdated(EntityUid uid, GasOutletInjectorComponent injector, ref AtmosDeviceUpdateEvent args)
        {
            if (!injector.Enabled)
                return;

            if (!_nodeContainer.TryGetNode(uid, injector.InletName, out PipeNode? inlet))
                return;

            var environment = _atmosphereSystem.GetContainingMixture(uid, args.Grid, args.Map, true, true);

            if (environment == null)
                return;

            if (inlet.Air.Temperature < 65)
                return;

            if (environment.Pressure > injector.MaxPressure)
                return;

            var timeDelta = args.dt;

            // TODO adjust ratio so that environment does not go above MaxPressure?
            var ratio = MathF.Min(65f, timeDelta * injector.TransferRate * _atmosphereSystem.PumpSpeedup() / inlet.Air.Volume);
            var removed = inlet.Air.RemoveRatio(ratio);

            _atmosphereSystem.Merge(environment, removed);
        }
    }
}