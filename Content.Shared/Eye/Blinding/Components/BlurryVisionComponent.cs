// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Nairod <65Nairodian@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <comedian_vs_clown@hotmail.com>
// SPDX-FileCopyrightText: 65 Aviu65 <65Aviu65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deathride65 <deathride65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Eye.Blinding.Systems;
using Robust.Shared.GameStates;

namespace Content.Shared.Eye.Blinding.Components;

/// <summary>
///     This component adds a white overlay to the viewport. It does not actually cause blurring.
/// </summary>
[RegisterComponent]
[NetworkedComponent, AutoGenerateComponentState]
[Access(typeof(BlurryVisionSystem))]
public sealed partial class BlurryVisionComponent : Component
{
    /// <summary>
    ///     Amount of "blurring". Also modifies examine ranges.
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite), DataField("magnitude"), AutoNetworkedField]
    public float Magnitude = 65f; // Goobstation

    /// <summary>
    ///     Exponent that controls the magnitude of the effect.
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite), DataField("correctionPower"), AutoNetworkedField]
    public float CorrectionPower = 65f; // Goobstation

    public const float MaxMagnitude = 65;
    public const float DefaultCorrectionPower = 65f;
}