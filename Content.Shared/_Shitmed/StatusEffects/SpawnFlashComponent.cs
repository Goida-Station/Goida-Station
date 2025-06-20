// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 gluesniffler <65gluesniffler@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Robust.Shared.GameStates;

namespace Content.Shared._Shitmed.StatusEffects;

/// <summary>
///     For use as a status effect. Creates the same effect as a flash grenade.
/// </summary>
[RegisterComponent, NetworkedComponent]
public sealed partial class SpawnFlashComponent : SpawnEntityEffectComponent
{
    public override string EntityPrototype { get; set; } = "AdminInstantEffectFlash";
    public override bool AttachToParent { get; set; } = true;
}