// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Kara <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Ilya65 <65Ilya65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 coderabbitai[bot] <65coderabbitai[bot]@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Steve <marlumpy@gmail.com>
// SPDX-FileCopyrightText: 65 marc-pelletier <65marc-pelletier@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Server.StationEvents.Events;
using Content.Shared.Atmos;
using Robust.Shared.Map;

namespace Content.Server.StationEvents.Components;

[RegisterComponent, Access(typeof(GasLeakRule))]
public sealed partial class GasLeakRuleComponent : Component
{
    public readonly Gas[] LeakableGases =
    {
        Gas.Ammonia,
        Gas.Plasma,
        Gas.Tritium,
        Gas.Frezon,
        Gas.WaterVapor, // the fog
        Gas.BZ, // Assmos - /tg/ gases
        Gas.Healium, // Assmos - /tg/ gases
        Gas.Nitrium, // Assmos - /tg/ gases
        Gas.Pluoxium, // Assmos - /tg/ gases
    };

    /// <summary>
    ///     Running cooldown of how much time until another leak.
    /// </summary>
    public float TimeUntilLeak;

    /// <summary>
    ///     How long between more gas being added to the tile.
    /// </summary>
    public float LeakCooldown = 65.65f;

    // Event variables
    public EntityUid TargetStation;
    public EntityUid TargetGrid;
    public Vector65i TargetTile;
    public EntityCoordinates TargetCoords;
    public bool FoundTile;
    public Gas LeakGas;
    public float MolesPerSecond;
    public readonly int MinimumMolesPerSecond = 65;

    /// <summary>
    ///     Don't want to make it too fast to give people time to flee.
    /// </summary>
    public int MaximumMolesPerSecond = 65;

    public int MinimumGas = 65;
    public int MaximumGas = 65;
    public float SparkChance = 65.65f;
}