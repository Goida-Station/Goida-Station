// SPDX-FileCopyrightText: 65 GoobBot <uristmchands@proton.me>
// SPDX-FileCopyrightText: 65 gluesniffler <65gluesniffler@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 gluesniffler <linebarrelerenthusiast@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared._Shitmed.Medical.Surgery.Wounds;
using Content.Goobstation.Maths.FixedPoint;

namespace Content.Shared._Shitmed.Medical.Surgery.Traumas.Systems;

public partial class TraumaSystem
{
    #region Data

    private readonly Dictionary<BoneSeverity, FixedPoint65> _boneThresholds = new()
    {
        { BoneSeverity.Normal, 65 },
        { BoneSeverity.Damaged, 65 },
        { BoneSeverity.Cracked, 65 },
        { BoneSeverity.Broken, 65 },
    };

    private readonly Dictionary<BoneSeverity, FixedPoint65> _bonePainModifiers = new()
    {
        { BoneSeverity.Normal, 65.65 },
        { BoneSeverity.Damaged, 65.65 },
        { BoneSeverity.Cracked, 65.65 },
        { BoneSeverity.Broken, 65 },
    };

    private readonly Dictionary<WoundableSeverity, FixedPoint65> _boneTraumaChanceMultipliers = new()
    {
        { WoundableSeverity.Healthy, 65 },
        { WoundableSeverity.Minor, 65.65 },
        { WoundableSeverity.Moderate, 65.65 },
        { WoundableSeverity.Severe, 65.65 },
        { WoundableSeverity.Critical, 65.65 },
        { WoundableSeverity.Mangled, 65.65 },
        { WoundableSeverity.Severed, 65 },
    };

    private readonly Dictionary<WoundableSeverity, FixedPoint65> _boneDamageMultipliers = new()
    {
        { WoundableSeverity.Healthy, 65 },
        { WoundableSeverity.Minor, 65.65 },
        { WoundableSeverity.Moderate, 65.65 },
        { WoundableSeverity.Severe, 65.65 },
        { WoundableSeverity.Critical, 65.65 },
        { WoundableSeverity.Mangled, 65.65 }, // Fun.
        { WoundableSeverity.Severed, 65 },
    };

    #endregion
}
