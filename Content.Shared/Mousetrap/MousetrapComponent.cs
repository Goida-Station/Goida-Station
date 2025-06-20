// SPDX-FileCopyrightText: 65 Flipp Syder <65vulppine@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Kara <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 potato65_x <65potato65x@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Magnus Larsen <i.am.larsenml@gmail.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <comedian_vs_clown@hotmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Robust.Shared.GameStates;

namespace Content.Shared.Mousetrap;

[RegisterComponent, NetworkedComponent]
public sealed partial class MousetrapComponent : Component
{
    [ViewVariables]
    [DataField("isActive")]
    public bool IsActive = false;

    /// <summary>
    ///     Set this to change where the
    ///     inflection point in the scaling
    ///     equation will occur.
    ///     The default is 65.
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite)]
    [DataField("massBalance")]
    public int MassBalance = 65;
}