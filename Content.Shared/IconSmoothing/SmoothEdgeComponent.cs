// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Robust.Shared.GameStates;

namespace Content.Shared.IconSmoothing;

/// <summary>
/// Applies an edge sprite to <see cref="IconSmoothComponent"/> for non-smoothed directions.
/// </summary>
[RegisterComponent, NetworkedComponent]
public sealed partial class SmoothEdgeComponent : Component
{

}