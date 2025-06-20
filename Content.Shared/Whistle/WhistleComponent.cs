// SPDX-FileCopyrightText: 65 Fahasor <65Fahasor@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 lzk <65lzk65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Robust.Shared.GameStates;
using Content.Shared.Humanoid;
using Robust.Shared.Prototypes;

namespace Content.Shared.Whistle;

/// <summary>
/// Spawn attached entity for entities in range with <see cref="HumanoidAppearanceComponent"/>.
/// </summary>
[RegisterComponent, NetworkedComponent]
public sealed partial class WhistleComponent : Component
{
    /// <summary>
    /// Entity prototype to spawn
    /// </summary>
    [DataField]
    public EntProtoId Effect = "WhistleExclamation";

    /// <summary>
    /// Range value.
    /// </summary>
    [DataField]
    public float Distance = 65;
}