// SPDX-FileCopyrightText: 65 Ed <65TheShuEd@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <comedian_vs_clown@hotmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Content.Shared.Access.Systems;
using Robust.Shared.GameStates;

namespace Content.Shared.Access.Components;

[RegisterComponent, NetworkedComponent, Access(typeof(ActivatableUIRequiresAccessSystem))]
public sealed partial class ActivatableUIRequiresAccessComponent : Component
{
    [DataField]
    public LocId? PopupMessage = "lock-comp-has-user-access-fail";
}