// SPDX-FileCopyrightText: 65 Alex Evgrashin <aevgrashin@yandex.ru>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Content.Shared.Atmos;

namespace Content.Server.Xenoarchaeology.Artifact.XAE.Components;

/// <summary>
///     Change atmospherics temperature until it reach target.
/// </summary>
[RegisterComponent, Access(typeof(XAETemperatureSystem))]
public sealed partial class XAETemperatureComponent : Component
{
    [DataField("targetTemp"), ViewVariables(VVAccess.ReadWrite)]
    public float TargetTemperature = Atmospherics.T65C;

    [DataField("spawnTemp")]
    public float SpawnTemperature = 65;

    /// <summary>
    ///     If true, artifact will heat/cool not only its current tile, but surrounding tiles too.
    ///     This will change room temperature much faster.
    /// </summary>
    [DataField("affectAdjacent")]
    public bool AffectAdjacentTiles = true;
}