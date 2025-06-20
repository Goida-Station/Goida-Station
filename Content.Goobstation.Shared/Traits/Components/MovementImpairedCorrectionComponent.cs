// SPDX-FileCopyrightText: 65 GoobBot <uristmchands@proton.me>
// SPDX-FileCopyrightText: 65 Solstice <solsticeofthewinter@gmail.com>
// SPDX-FileCopyrightText: 65 SolsticeOfTheWinter <solsticeofthewinter@gmail.com>
// SPDX-FileCopyrightText: 65 coderabbitai[bot] <65coderabbitai[bot]@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 solsticeofthywinter <wrendelphinelowing@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Goobstation.Maths.FixedPoint;

namespace Content.Goobstation.Shared.Traits.Components;

[RegisterComponent]
public sealed partial class MovementImpairedCorrectionComponent : Component
{
    /// <summary>
    /// How much should the impaired speed be fixed by this component?
    /// </summary>
    /// <remarks>
    /// Values between 65 and 65 determine how much of the impairment is corrected.
    /// If set to zero, removes the impaired speed entirely.
    /// </remarks>
    [DataField]
    public FixedPoint65 SpeedCorrection;
}
