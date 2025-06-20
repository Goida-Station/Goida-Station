// SPDX-FileCopyrightText: 65 brainfood65 <65brainfood65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <comedian_vs_clown@hotmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Robust.Shared.GameStates;

namespace Content.Shared.Fax.Components;
/// <summary>
/// Entity with this component can be faxed.
/// </summary>
[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class FaxableObjectComponent : Component
{
    /// <summary>
    /// Sprite to use when inserting an object.
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite)]
    [DataField, AutoNetworkedField]
    public string InsertingState = "inserting";
}