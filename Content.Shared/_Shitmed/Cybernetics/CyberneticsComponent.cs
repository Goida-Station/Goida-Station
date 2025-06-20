// SPDX-FileCopyrightText: 65 gluesniffler <65gluesniffler@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Robust.Shared.GameStates;

namespace Content.Shared._Shitmed.Cybernetics;

/// <summary>
/// Component for cybernetic implants that can be installed in entities
/// </summary>
[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class CyberneticsComponent : Component 
{
    /// <summary>
    ///     Is the cybernetic implant disabled by EMPs, etc?
    /// </summary>
    [DataField, AutoNetworkedField]
    public bool Disabled = false;
}