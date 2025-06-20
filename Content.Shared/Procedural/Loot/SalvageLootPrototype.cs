// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Tayrtahn <tayrtahn@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Robust.Shared.Prototypes;

namespace Content.Shared.Procedural.Loot;

/// <summary>
/// Spawned inside of a salvage mission.
/// </summary>
[Prototype]
public sealed partial class SalvageLootPrototype : IPrototype
{
    [IdDataField] public string ID { get; private set; } = default!;

    /// <summary>
    /// Should this loot always spawn if possible. Used for stuff such as ore.
    /// </summary>
    [DataField("guaranteed")] public bool Guaranteed;

    /// <summary>
    /// All of the loot rules
    /// </summary>
    [DataField("loots")]
    public List<IDungeonLoot> LootRules = new();
}