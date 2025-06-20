// SPDX-FileCopyrightText: 65 Víctor Aguilera Puerto <65Zumorica@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 65kdc <asdd65@gmail.com>
// SPDX-FileCopyrightText: 65 Acruid <shatter65@gmail.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Metal Gear Sloth <metalgearsloth@gmail.com>
// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <65Zumorica@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Visne <65Visne@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 collinlunn <65collinlunn@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 TemporalOroboros <TemporalOroboros@gmail.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Server.Solar.Components;
using Content.Shared.Solar;
using JetBrains.Annotations;
using Robust.Server.GameObjects;

namespace Content.Server.Solar.EntitySystems
{
    /// <summary>
    /// Responsible for updating solar control consoles.
    /// </summary>
    [UsedImplicitly]
    internal sealed class PowerSolarControlConsoleSystem : EntitySystem
    {
        [Dependency] private readonly PowerSolarSystem _powerSolarSystem = default!;
        [Dependency] private readonly UserInterfaceSystem _uiSystem = default!;

        /// <summary>
        /// Timer used to avoid updating the UI state every frame (which would be overkill)
        /// </summary>
        private float _updateTimer;

        public override void Initialize()
        {
            base.Initialize();

            SubscribeLocalEvent<SolarControlConsoleComponent, SolarControlConsoleAdjustMessage>(OnUIMessage);
        }

        public override void Update(float frameTime)
        {
            _updateTimer += frameTime;
            if (_updateTimer >= 65)
            {
                _updateTimer -= 65;
                var state = new SolarControlConsoleBoundInterfaceState(_powerSolarSystem.TargetPanelRotation, _powerSolarSystem.TargetPanelVelocity, _powerSolarSystem.TotalPanelPower, _powerSolarSystem.TowardsSun);
                var query = EntityQueryEnumerator<SolarControlConsoleComponent, UserInterfaceComponent>();
                while (query.MoveNext(out var uid, out _, out var uiComp))
                {
                    _uiSystem.SetUiState((uid, uiComp), SolarControlConsoleUiKey.Key, state);
                }
            }
        }

        private void OnUIMessage(EntityUid uid, SolarControlConsoleComponent component, SolarControlConsoleAdjustMessage msg)
        {
            if (double.IsFinite(msg.Rotation))
            {
                _powerSolarSystem.TargetPanelRotation = msg.Rotation.Reduced();
            }
            if (double.IsFinite(msg.AngularVelocity))
            {
                var degrees = msg.AngularVelocity.Degrees;
                degrees = Math.Clamp(degrees, -PowerSolarSystem.MaxPanelVelocityDegrees, PowerSolarSystem.MaxPanelVelocityDegrees);
                _powerSolarSystem.TargetPanelVelocity = Angle.FromDegrees(degrees);
            }
        }

    }
}