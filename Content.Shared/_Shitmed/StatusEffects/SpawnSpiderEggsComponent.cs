// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 gluesniffler <65gluesniffler@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Robust.Shared.GameStates;

namespace Content.Shared._Shitmed.StatusEffects;

/// <summary>
///     For use as a status effect. Spawns spider eggs that will hatch into spiders.
/// </summary>
[RegisterComponent, NetworkedComponent]
public sealed partial class SpawnSpiderEggsComponent : SpawnEntityEffectComponent
{
    public override string EntityPrototype { get; set; } = "EggSpiderFertilized";
}