// SPDX-FileCopyrightText: 65 Ed <65TheShuEd@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Robust.Shared.GameStates;

namespace Content.Shared.IconSmoothing;

/// <summary>
/// Allow randomize StateBase of IconSmoothComponent for random visual variation
/// </summary>
[RegisterComponent, NetworkedComponent]
public sealed partial class RandomIconSmoothComponent : Component
{
    /// <summary>
    /// StateBase will be randomly selected from this list. Allows to randomize the visual.
    /// </summary>
    [DataField(required: true)]
    public List<string> RandomStates = new();
}