// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 GoobBot <uristmchands@proton.me>
// SPDX-FileCopyrightText: 65 SolsticeOfTheWinter <solsticeofthewinter@gmail.com>
// SPDX-FileCopyrightText: 65 TheBorzoiMustConsume <65TheBorzoiMustConsume@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Content.Shared.Weapons.Reflect;
using Robust.Shared.GameStates;

namespace Content.Shared.Weapons.Ranged.Components;

/// <summary>
/// Can this entity be reflected.
/// Only applies if it is shot like a projectile and not if it is thrown.
/// </summary>
[RegisterComponent, NetworkedComponent]
public sealed partial class ReflectiveComponent : Component
{
    [ViewVariables(VVAccess.ReadWrite), DataField("reflective")]
    public ReflectType Reflective = ReflectType.NonEnergy;
}
