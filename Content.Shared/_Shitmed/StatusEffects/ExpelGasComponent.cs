// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 gluesniffler <65gluesniffler@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Atmos;
using Robust.Shared.GameStates;
namespace Content.Shared._Shitmed.StatusEffects;

/// <summary>
///     Randomly spawns gas of a given type.
/// </summary>
[RegisterComponent]
public sealed partial class ExpelGasComponent : Component
{
    public List<Gas> PossibleGases = new()
    {
        Gas.Oxygen,
        Gas.Plasma,
        Gas.Nitrogen,
        Gas.CarbonDioxide,
        Gas.Tritium,
        Gas.Ammonia,
        Gas.NitrousOxide,
        Gas.Frezon,
        Gas.BZ, ///tg/ gases
        Gas.Healium, ///tg/ gases
        Gas.Nitrium, ///tg/ gases
    };
}