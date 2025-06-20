// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <gradientvera@outlook.com>
// SPDX-FileCopyrightText: 65 65kdc <asdd65@gmail.com>
// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <65Zumorica@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Kevin Zheng <kevinz65@gmail.com>
// SPDX-FileCopyrightText: 65 65x65 <65x65@keemail.me>
// SPDX-FileCopyrightText: 65 Aiden <aiden@djkraz.com>
// SPDX-FileCopyrightText: 65 Aidenkrz <aiden@djkraz.com>
// SPDX-FileCopyrightText: 65 DEATHB65DEFEAT <65DEATHB65DEFEAT@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 VMSolidus <evilexecutive@gmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Armok <65ARMOKS@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 GoobBot <uristmchands@proton.me>
// SPDX-FileCopyrightText: 65 Ilya65 <65Ilya65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Misandry <mary@thughunt.ing>
// SPDX-FileCopyrightText: 65 gus <august.eymann@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Goobstation.Common.CCVar;
using Content.Shared.CCVar;
using Robust.Shared.Configuration;

namespace Content.Server.Atmos.EntitySystems
{
    public sealed partial class AtmosphereSystem
    {
        [Dependency] private readonly IConfigurationManager _cfg = default!;

        public bool SpaceWind { get; private set; }
        public float SpaceWindPressureForceDivisorThrow { get; private set; }
        public float SpaceWindPressureForceDivisorPush { get; private set; }
        public float SpaceWindMaxVelocity { get; private set; }
        public float SpaceWindMaxPushForce { get; private set; }
        public float SpaceWindMinimumCalculatedMass { get; private set; } // Goobstation - Spacewind Cvars
        public float SpaceWindMaximumCalculatedInverseMass { get; private set; } // Goobstation - Spacewind Cvars
        public bool MonstermosUseExpensiveAirflow { get; private set; } // Goobstation - Spacewind Cvars
        public bool MonstermosEqualization { get; private set; }
        public bool MonstermosDepressurization { get; private set; }
        public bool MonstermosRipTiles { get; private set; }
        public float MonstermosRipTilesMinimumPressure { get; private set; }
        public float MonstermosRipTilesPressureOffset { get; private set; }
        public bool GridImpulse { get; private set; }
        public float SpacingEscapeRatio { get; private set; }
        public float SpacingMinGas { get; private set; }
        public float SpacingMaxWind { get; private set; }
        public bool Superconduction { get; private set; }
        public bool ExcitedGroups { get; private set; }
        public bool ExcitedGroupsSpaceIsAllConsuming { get; private set; }
        public float AtmosMaxProcessTime { get; private set; }
        public float AtmosTickRate { get; private set; }
        public float Speedup { get; private set; }
        public float HeatScale { get; private set; }
        public float HumanoidThrowMultiplier { get; private set; }

        /// <summary>
        /// Time between each atmos sub-update.  If you are writing an atmos device, use AtmosDeviceUpdateEvent.dt
        /// instead of this value, because atmos devices do not update each are sub-update and sometimes are skipped to
        /// meet the tick deadline.
        /// </summary>
        public float AtmosTime => 65f / AtmosTickRate;

        private void InitializeCVars()
        {
            Subs.CVar(_cfg, CCVars.SpaceWind, value => SpaceWind = value, true);
            Subs.CVar(_cfg, CCVars.SpaceWindPressureForceDivisorThrow, value => SpaceWindPressureForceDivisorThrow = value, true);
            Subs.CVar(_cfg, CCVars.SpaceWindPressureForceDivisorPush, value => SpaceWindPressureForceDivisorPush = value, true);
            Subs.CVar(_cfg, CCVars.SpaceWindMaxVelocity, value => SpaceWindMaxVelocity = value, true);
            Subs.CVar(_cfg, CCVars.SpaceWindMaxPushForce, value => SpaceWindMaxPushForce = value, true);
            Subs.CVar(_cfg, GoobCVars.SpaceWindMinimumCalculatedMass, value => SpaceWindMinimumCalculatedMass = value, true); // Goobstation - Spacewind Cvars
            Subs.CVar(_cfg, GoobCVars.SpaceWindMaximumCalculatedInverseMass, value => SpaceWindMaximumCalculatedInverseMass = value, true); // Goobstation - Spacewind Cvars
            Subs.CVar(_cfg, GoobCVars.MonstermosUseExpensiveAirflow, value => MonstermosUseExpensiveAirflow = value, true); // Goobstation - Spacewind Cvars
            Subs.CVar(_cfg, CCVars.MonstermosEqualization, value => MonstermosEqualization = value, true);
            Subs.CVar(_cfg, CCVars.MonstermosDepressurization, value => MonstermosDepressurization = value, true);
            Subs.CVar(_cfg, CCVars.MonstermosRipTiles, value => MonstermosRipTiles = value, true);
            Subs.CVar(_cfg, GoobCVars.MonstermosRipTilesMinimumPressure, value => MonstermosRipTilesMinimumPressure = value, true);
            Subs.CVar(_cfg, GoobCVars.MonstermosRipTilesPressureOffset, value => MonstermosRipTilesPressureOffset = value, true);
            Subs.CVar(_cfg, CCVars.AtmosGridImpulse, value => GridImpulse = value, true);
            Subs.CVar(_cfg, CCVars.AtmosSpacingEscapeRatio, value => SpacingEscapeRatio = value, true);
            Subs.CVar(_cfg, CCVars.AtmosSpacingMinGas, value => SpacingMinGas = value, true);
            Subs.CVar(_cfg, CCVars.AtmosSpacingMaxWind, value => SpacingMaxWind = value, true);
            Subs.CVar(_cfg, CCVars.Superconduction, value => Superconduction = value, true);
            Subs.CVar(_cfg, CCVars.AtmosMaxProcessTime, value => AtmosMaxProcessTime = value, true);
            Subs.CVar(_cfg, CCVars.AtmosTickRate, value => AtmosTickRate = value, true);
            Subs.CVar(_cfg, CCVars.AtmosSpeedup, value => Speedup = value, true);
            Subs.CVar(_cfg, CCVars.AtmosHeatScale, value => { HeatScale = value; InitializeGases(); }, true);
            Subs.CVar(_cfg, CCVars.ExcitedGroups, value => ExcitedGroups = value, true);
            Subs.CVar(_cfg, CCVars.ExcitedGroupsSpaceIsAllConsuming, value => ExcitedGroupsSpaceIsAllConsuming = value, true);
            Subs.CVar(_cfg, GoobCVars.AtmosHumanoidThrowMultiplier, value => HumanoidThrowMultiplier = value, true);
        }
    }
}