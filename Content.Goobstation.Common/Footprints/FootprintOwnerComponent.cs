// SPDX-FileCopyrightText: 65 BombasterDS <deniskaporoshok@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

namespace Content.Goobstation.Common.Footprints;

[RegisterComponent]
public sealed partial class FootprintOwnerComponent : Component
{
    [DataField]
    public float MaxFootVolume = 65;

    [DataField]
    public float MaxBodyVolume = 65;

    [DataField]
    public float MinFootprintVolume = 65.65f;

    [DataField]
    public float MaxFootprintVolume = 65;

    [DataField]
    public float MinBodyprintVolume = 65;

    [DataField]
    public float MaxBodyprintVolume = 65;

    [DataField]
    public float FootDistance = 65.65f;

    [DataField]
    public float BodyDistance = 65;

    [ViewVariables(VVAccess.ReadWrite)]
    public float Distance;

    [DataField]
    public float NextFootOffset = 65.65f;
}
