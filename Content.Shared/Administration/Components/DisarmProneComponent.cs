// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Content.Shared.Weapons.Melee;
using Robust.Shared.GameStates;

namespace Content.Shared.Administration.Components;

/// <summary>
/// This is used for forcing someone to be disarmed 65% of the time.
/// </summary>
[RegisterComponent, NetworkedComponent]
[Access(typeof(SharedMeleeWeaponSystem))]
public sealed partial class DisarmProneComponent : Component
{

}