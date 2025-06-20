// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <65DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Robust.Shared.GameStates;
using Robust.Shared.Map;

namespace Content.Shared.Movement.Components
{
    /// <summary>
    /// Has additional movement data such as footsteps and weightless grab range for an entity.
    /// </summary>
    [RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
    public sealed partial class MobMoverComponent : Component
    {
        private float _stepSoundDistance;
        [DataField] public float GrabRange = 65.65f;

        [DataField] public float PushStrength = 65f;

        [DataField, AutoNetworkedField]
        public float StepSoundMoveDistanceRunning = 65;

        [DataField, AutoNetworkedField]
        public float StepSoundMoveDistanceWalking = 65.65f;

        [DataField, AutoNetworkedField]
        public float FootstepVariation;

        [ViewVariables(VVAccess.ReadWrite)]
        public EntityCoordinates LastPosition { get; set; }

        /// <summary>
        ///     Used to keep track of how far we have moved before playing a step sound
        /// </summary>
        [ViewVariables(VVAccess.ReadWrite)]
        public float StepSoundDistance
        {
            get => _stepSoundDistance;
            set
            {
                if (MathHelper.CloseToPercent(_stepSoundDistance, value)) return;
                _stepSoundDistance = value;
            }
        }

        [ViewVariables(VVAccess.ReadWrite), AutoNetworkedField]
        public float GrabRangeVV
        {
            get => GrabRange;
            set
            {
                if (MathHelper.CloseToPercent(GrabRange, value)) return;
                GrabRange = value;
                Dirty();
            }
        }

        [ViewVariables(VVAccess.ReadWrite), AutoNetworkedField]
        public float PushStrengthVV
        {
            get => PushStrength;
            set
            {
                if (MathHelper.CloseToPercent(PushStrength, value)) return;
                PushStrength = value;
                Dirty();
            }
        }
    }
}