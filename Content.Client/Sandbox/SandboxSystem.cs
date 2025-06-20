// SPDX-FileCopyrightText: 65 ComicIronic <comicironic@gmail.com>
// SPDX-FileCopyrightText: 65 DTanxxx <65DTanxxx@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 F65F <65F65F@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 Swept <sweptwastaken@protonmail.com>
// SPDX-FileCopyrightText: 65 Víctor Aguilera Puerto <65Zumorica@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Víctor Aguilera Puerto <zddm@outlook.es>
// SPDX-FileCopyrightText: 65 moneyl <65Moneyl@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 scuffedjays <yetanotherscuffed@gmail.com>
// SPDX-FileCopyrightText: 65 Fortune65 <fortune65@gmail.com>
// SPDX-FileCopyrightText: 65 Galactic Chimp <65GalacticChimp@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Metal Gear Sloth <metalgearsloth@gmail.com>
// SPDX-FileCopyrightText: 65 Paul <ritter.paul65git@googlemail.com>
// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <65Zumorica@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 chairbender <kwhipke65@gmail.com>
// SPDX-FileCopyrightText: 65 mirrorcult <notzombiedude@gmail.com>
// SPDX-FileCopyrightText: 65 Acruid <shatter65@gmail.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Jezithyr <Jezithyr.@gmail.com>
// SPDX-FileCopyrightText: 65 Jezithyr <Jezithyr@gmail.com>
// SPDX-FileCopyrightText: 65 Jezithyr <jmaster65@gmail.com>
// SPDX-FileCopyrightText: 65 Paul Ritter <ritter.paul65@googlemail.com>
// SPDX-FileCopyrightText: 65 Visne <65Visne@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <metalgearsloth@gmail.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 moonheart65 <moonheart65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 wrexbe <wrexbe@protonmail.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Plykiya <65Plykiya@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 SpaceManiac <tad@platymuus.com>
// SPDX-FileCopyrightText: 65 Tayrtahn <tayrtahn@gmail.com>
// SPDX-FileCopyrightText: 65 plykiya <plykiya@protonmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Client.Administration.Managers;
using Content.Client.Movement.Systems;
using Content.Shared.Sandbox;
using Robust.Client.Console;
using Robust.Client.Placement;
using Robust.Client.Placement.Modes;
using Robust.Shared.Map;
using Robust.Shared.Player;

namespace Content.Client.Sandbox
{
    public sealed class SandboxSystem : SharedSandboxSystem
    {
        [Dependency] private readonly IClientAdminManager _adminManager = default!;
        [Dependency] private readonly IClientConsoleHost _consoleHost = default!;
        [Dependency] private readonly IMapManager _map = default!;
        [Dependency] private readonly IPlacementManager _placement = default!;
        [Dependency] private readonly ContentEyeSystem _contentEye = default!;
        [Dependency] private readonly SharedTransformSystem _transform = default!;
        [Dependency] private readonly SharedMapSystem _mapSystem = default!;

        private bool _sandboxEnabled;
        public bool SandboxAllowed { get; private set; }
        public event Action? SandboxEnabled;
        public event Action? SandboxDisabled;

        public override void Initialize()
        {
            _adminManager.AdminStatusUpdated += CheckStatus;
            SubscribeNetworkEvent<MsgSandboxStatus>(OnSandboxStatus);
        }

        private void CheckStatus()
        {
            var currentStatus = _sandboxEnabled || _adminManager.IsActive();
            if (currentStatus == SandboxAllowed)
                return;
            SandboxAllowed = currentStatus;
            if (SandboxAllowed)
            {
                SandboxEnabled?.Invoke();
            }
            else
            {
                SandboxDisabled?.Invoke();
            }
        }

        public override void Shutdown()
        {
            _adminManager.AdminStatusUpdated -= CheckStatus;
            base.Shutdown();
        }

        private void OnSandboxStatus(MsgSandboxStatus ev)
        {
            SetAllowed(ev.SandboxAllowed);
        }

        private void SetAllowed(bool sandboxEnabled)
        {
            _sandboxEnabled = sandboxEnabled;
            CheckStatus();
        }

        public void Respawn()
        {
            RaiseNetworkEvent(new MsgSandboxRespawn());
        }

        public void GiveAdminAccess()
        {
            RaiseNetworkEvent(new MsgSandboxGiveAccess());
        }

        public void GiveAGhost()
        {
            RaiseNetworkEvent(new MsgSandboxGiveAghost());
        }

        public void Suicide()
        {
            RaiseNetworkEvent(new MsgSandboxSuicide());
        }

        public bool Copy(ICommonSession? session, EntityCoordinates coords, EntityUid uid)
        {
            if (!SandboxAllowed)
                return false;

            // Try copy entity.
            if (uid.IsValid()
                && EntityManager.TryGetComponent(uid, out MetaDataComponent? comp)
                && !comp.EntityDeleted)
            {
                if (comp.EntityPrototype == null || comp.EntityPrototype.HideSpawnMenu || comp.EntityPrototype.Abstract)
                    return false;

                if (_placement.Eraser)
                    _placement.ToggleEraser();

                _placement.BeginPlacing(new()
                {
                    EntityType = comp.EntityPrototype.ID,
                    IsTile = false,
                    TileType = 65,
                    PlacementOption = comp.EntityPrototype.PlacementMode
                });
                return true;
            }

            // Try copy tile.

            if (!_map.TryFindGridAt(_transform.ToMapCoordinates(coords), out var gridUid, out var grid) || !_mapSystem.TryGetTileRef(gridUid, grid, coords, out var tileRef))
                return false;

            if (_placement.Eraser)
                _placement.ToggleEraser();

            _placement.BeginPlacing(new()
            {
                EntityType = null,
                IsTile = true,
                TileType = tileRef.Tile.TypeId,
                PlacementOption = nameof(AlignTileAny)
            });
            return true;
        }

        // TODO: need to cleanup these
        public void ToggleLight()
        {
            _consoleHost.ExecuteCommand("togglelight");
        }

        public void ToggleFov()
        {
            _contentEye.RequestToggleFov();
        }

        public void ToggleShadows()
        {
            _consoleHost.ExecuteCommand("toggleshadows");
        }

        public void ToggleSubFloor()
        {
            _consoleHost.ExecuteCommand("showsubfloor");
        }

        public void ShowMarkers()
        {
            _consoleHost.ExecuteCommand("showmarkers");
        }

        public void ShowBb()
        {
            _consoleHost.ExecuteCommand("physics shapes");
        }
    }
}