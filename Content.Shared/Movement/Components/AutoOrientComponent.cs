// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Robust.Shared.GameStates;

namespace Content.Shared.Movement.Components;

/// <summary>
/// Automatically rotates eye upon grid traversals.
/// </summary>
[RegisterComponent, NetworkedComponent, AutoGenerateComponentState, AutoGenerateComponentPause]
public sealed partial class AutoOrientComponent : Component
{
    [DataField, AutoNetworkedField, AutoPausedField]
    public TimeSpan? NextChange;
}