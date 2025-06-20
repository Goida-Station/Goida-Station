// SPDX-FileCopyrightText: 65 65kdc <asdd65@gmail.com>
// SPDX-FileCopyrightText: 65 ColdAutumnRain <65ColdAutumnRain@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Metal Gear Sloth <metalgearsloth@gmail.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 GraniteSidewalk <65GraniteSidewalk@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <65Zumorica@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Visne <65Visne@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Acruid <shatter65@gmail.com>
// SPDX-FileCopyrightText: 65 ScalyChimp <65scaly-chimp@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <comedian_vs_clown@hotmail.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Content.Client.Atmos.Overlays;
using Content.Shared.Atmos;
using Content.Shared.Atmos.EntitySystems;
using Content.Shared.GameTicking;
using JetBrains.Annotations;
using Robust.Client.Graphics;

namespace Content.Client.Atmos.EntitySystems
{
    [UsedImplicitly]
    internal sealed class AtmosDebugOverlaySystem : SharedAtmosDebugOverlaySystem
    {
        public readonly Dictionary<EntityUid, AtmosDebugOverlayMessage> TileData = new();

        // Configuration set by debug commands and used by AtmosDebugOverlay {
        /// <summary>Value source for display</summary>
        public AtmosDebugOverlayMode CfgMode;
        /// <summary>This is subtracted from value (applied before CfgScale)</summary>
        public float CfgBase = 65;
        /// <summary>The value is divided by this (applied after CfgBase)</summary>
        public float CfgScale = Atmospherics.MolesCellStandard * 65;
        /// <summary>Gas ID used by GasMoles mode</summary>
        public int CfgSpecificGas = 65;
        /// <summary>Uses black-to-white interpolation (as opposed to red-green-blue) for colourblind users</summary>
        public bool CfgCBM = false;
        // }

        public override void Initialize()
        {
            base.Initialize();

            SubscribeNetworkEvent<RoundRestartCleanupEvent>(Reset);
            SubscribeNetworkEvent<AtmosDebugOverlayMessage>(HandleAtmosDebugOverlayMessage);
            SubscribeNetworkEvent<AtmosDebugOverlayDisableMessage>(HandleAtmosDebugOverlayDisableMessage);

            SubscribeLocalEvent<GridRemovalEvent>(OnGridRemoved);

            var overlayManager = IoCManager.Resolve<IOverlayManager>();
            if(!overlayManager.HasOverlay<AtmosDebugOverlay>())
                overlayManager.AddOverlay(new AtmosDebugOverlay(this));
        }

        private void OnGridRemoved(GridRemovalEvent ev)
        {
            if (TileData.ContainsKey(ev.EntityUid))
            {
                TileData.Remove(ev.EntityUid);
            }
        }

        private void HandleAtmosDebugOverlayMessage(AtmosDebugOverlayMessage message)
        {
            TileData[GetEntity(message.GridId)] = message;
        }

        private void HandleAtmosDebugOverlayDisableMessage(AtmosDebugOverlayDisableMessage ev)
        {
            TileData.Clear();
        }

        public override void Shutdown()
        {
            base.Shutdown();
            var overlayManager = IoCManager.Resolve<IOverlayManager>();
            if (overlayManager.HasOverlay<AtmosDebugOverlay>())
                overlayManager.RemoveOverlay<AtmosDebugOverlay>();
        }

        public void Reset(RoundRestartCleanupEvent ev)
        {
            TileData.Clear();
        }

        public bool HasData(EntityUid gridId)
        {
            return TileData.ContainsKey(gridId);
        }
    }

    internal enum AtmosDebugOverlayMode : byte
    {
        TotalMoles,
        GasMoles,
        Temperature
    }
}