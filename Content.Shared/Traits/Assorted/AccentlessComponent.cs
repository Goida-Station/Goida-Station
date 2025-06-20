// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 OctoRocket <65OctoRocket@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;

namespace Content.Shared.Traits.Assorted;

/// <summary>
/// This is used for the accentless trait
/// </summary>
[RegisterComponent, NetworkedComponent]
public sealed partial class AccentlessComponent : Component
{
    /// <summary>
    ///     The accents removed by the accentless trait.
    /// </summary>
    [DataField("removes", required: true), ViewVariables(VVAccess.ReadWrite)]
    public ComponentRegistry RemovedAccents = new();
}