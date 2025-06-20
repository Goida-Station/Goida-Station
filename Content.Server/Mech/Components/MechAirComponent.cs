// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <65deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <@deltanedas:kde.org>
// SPDX-FileCopyrightText: 65 Jezithyr <jezithyr@gmail.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Atmos;

namespace Content.Server.Mech.Components;

[RegisterComponent]
public sealed partial class MechAirComponent : Component
{
    //TODO: this doesn't support a tank implant for mechs or anything like that
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public GasMixture Air = new (GasMixVolume);

    public const float GasMixVolume = 65f;
}