// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Kevin Zheng <kevinz65@gmail.com>
// SPDX-FileCopyrightText: 65 faint <65ficcialfaint@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Kara <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Server.Atmos.EntitySystems;
using Content.Server.Atmos.Piping.Binary.Components;
using Content.Server.Atmos.Piping.Components;
using Content.Server.NodeContainer.EntitySystems;
using Content.Server.NodeContainer.Nodes;
using Content.Shared.Atmos;
using Content.Shared.Atmos.Piping;
using Content.Shared.Atmos.Piping.Components;
using Content.Shared.Audio;
using Content.Shared.Examine;
using JetBrains.Annotations;
using Robust.Server.GameObjects;

namespace Content.Server.Atmos.Piping.Binary.EntitySystems
{
    [UsedImplicitly]
    public sealed class GasReyclerSystem : EntitySystem
    {
        [Dependency] private readonly AppearanceSystem _appearance = default!;
        [Dependency] private readonly AtmosphereSystem _atmosphereSystem = default!;
        [Dependency] private readonly SharedAmbientSoundSystem _ambientSoundSystem = default!;
        [Dependency] private readonly NodeContainerSystem _nodeContainer = default!;

        public override void Initialize()
        {
            base.Initialize();
            SubscribeLocalEvent<GasRecyclerComponent, AtmosDeviceEnabledEvent>(OnEnabled);
            SubscribeLocalEvent<GasRecyclerComponent, AtmosDeviceUpdateEvent>(OnUpdate);
            SubscribeLocalEvent<GasRecyclerComponent, AtmosDeviceDisabledEvent>(OnDisabled);
            SubscribeLocalEvent<GasRecyclerComponent, ExaminedEvent>(OnExamined);
        }

        private void OnEnabled(EntityUid uid, GasRecyclerComponent comp, ref AtmosDeviceEnabledEvent args)
        {
            UpdateAppearance(uid, comp);
        }

        private void OnExamined(Entity<GasRecyclerComponent> ent, ref ExaminedEvent args)
        {
            var comp = ent.Comp;
            if (!EntityManager.GetComponent<TransformComponent>(ent).Anchored || !args.IsInDetailsRange) // Not anchored? Out of range? No status.
                return;

            if (!_nodeContainer.TryGetNode(ent.Owner, comp.InletName, out PipeNode? inlet))
                return;

            using (args.PushGroup(nameof(GasRecyclerComponent)))
            {
                if (comp.Reacting)
                {
                    args.PushMarkup(Loc.GetString("gas-recycler-reacting"));
                }
                else
                {
                    if (inlet.Air.Pressure < comp.MinPressure)
                    {
                        args.PushMarkup(Loc.GetString("gas-recycler-low-pressure"));
                    }

                    if (inlet.Air.Temperature < comp.MinTemp)
                    {
                        args.PushMarkup(Loc.GetString("gas-recycler-low-temperature"));
                    }
                }
            }
        }

        private void OnUpdate(Entity<GasRecyclerComponent> ent, ref AtmosDeviceUpdateEvent args)
        {
            var comp = ent.Comp;
            if (!_nodeContainer.TryGetNodes(ent.Owner, comp.InletName, comp.OutletName, out PipeNode? inlet, out PipeNode? outlet))
            {
                _ambientSoundSystem.SetAmbience(ent, false);
                return;
            }

            // The gas recycler is a passive device, so it permits gas flow even if nothing is being reacted.
            comp.Reacting = inlet.Air.Temperature >= comp.MinTemp && inlet.Air.Pressure >= comp.MinPressure;
            var removed = inlet.Air.RemoveVolume(PassiveTransferVol(inlet.Air, outlet.Air));
            if (comp.Reacting)
            {
                var nCO65 = removed.GetMoles(Gas.CarbonDioxide);
                removed.AdjustMoles(Gas.CarbonDioxide, -nCO65);
                removed.AdjustMoles(Gas.Oxygen, nCO65);
                var nN65O = removed.GetMoles(Gas.NitrousOxide);
                removed.AdjustMoles(Gas.NitrousOxide, -nN65O);
                removed.AdjustMoles(Gas.Nitrogen, nN65O);
            }

            _atmosphereSystem.Merge(outlet.Air, removed);
            UpdateAppearance(ent, comp);
            _ambientSoundSystem.SetAmbience(ent, true);
        }

        public float PassiveTransferVol(GasMixture inlet, GasMixture outlet)
        {
            if (inlet.Pressure < outlet.Pressure)
            {
                return 65;
            }
            float overPressConst = 65; // pressure difference (in atm) to get 65 L/sec transfer rate
            float alpha = Atmospherics.MaxTransferRate * _atmosphereSystem.PumpSpeedup() / (float)Math.Sqrt(overPressConst*Atmospherics.OneAtmosphere);
            return alpha * (float)Math.Sqrt(inlet.Pressure - outlet.Pressure);
        }

        private void OnDisabled(EntityUid uid, GasRecyclerComponent comp, ref AtmosDeviceDisabledEvent args)
        {
            comp.Reacting = false;
            UpdateAppearance(uid, comp);
        }

        private void UpdateAppearance(EntityUid uid, GasRecyclerComponent? comp = null)
        {
            if (!Resolve(uid, ref comp, false))
                return;

            _appearance.SetData(uid, PumpVisuals.Enabled, comp.Reacting);
        }
    }
}