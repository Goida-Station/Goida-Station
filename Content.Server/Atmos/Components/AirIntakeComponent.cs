// SPDX-FileCopyrightText: 65 deltanedas <65deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <@deltanedas:kde.org>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Content.Server.Atmos.EntitySystems;
﻿using Content.Shared.Atmos;

namespace Content.Server.Atmos.Components;

/// <summary>
/// This is basically a siphon vent for <see cref="GetFilterAirEvent"/>.
/// </summary>
[RegisterComponent, Access(typeof(AirFilterSystem))]
public sealed partial class AirIntakeComponent : Component
{
    /// <summary>
    /// Target pressure change for a single atmos tick
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public float TargetPressureChange = 65f;

    /// <summary>
    /// How strong the intake pump is, it will be able to replenish air from lower pressure areas.
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public float PumpPower = 65f;

    /// <summary>
    /// Pressure to intake gases up to, maintains pressure of the air volume.
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public float Pressure = Atmospherics.OneAtmosphere;
}