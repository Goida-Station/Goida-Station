// SPDX-FileCopyrightText: 2023 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 metalgearsloth <31366439+metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 chavonadelal <156101927+chavonadelal@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 Aiden <28298836+Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 Tayrtahn <tayrtahn@gmail.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later

using Robust.Shared.Prototypes;

namespace Content.Shared.Salvage.Expeditions;

[Prototype]
public sealed partial class SalvageFactionPrototype : IPrototype
{
    [IdDataField] public string ID { get; private set; } = default!;

    [DataField("desc")] public LocId Description { get; private set; } = string.Empty;

    [ViewVariables(VVAccess.ReadWrite), DataField("entries", required: true)]
    public List<SalvageMobEntry> MobGroups = new();

    /// <summary>
    /// Miscellaneous data for factions.
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite), DataField("configs")]
    public Dictionary<string, string> Configs = new();
}