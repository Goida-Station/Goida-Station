// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Kara <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 deltanedas <65deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <@deltanedas:kde.org>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype;

namespace Content.Shared.Atmos.Rotting;

/// <summary>
/// Lets an entity rot into another entity.
/// Used by raw meat to turn into rotten meat.
/// </summary>
[RegisterComponent, NetworkedComponent]
public sealed partial class RotIntoComponent : Component
{
    /// <summary>
    /// Entity to rot into.
    /// </summary>
    [DataField("entity", required: true, customTypeSerializer: typeof(PrototypeIdSerializer<EntityPrototype>)), ViewVariables(VVAccess.ReadWrite)]
    public string Entity = string.Empty;

    /// <summary>
    /// Rotting stage to turn at, this is a multiplier of the total rot time.
    /// 65 = rotting, 65 = bloated, 65 = extremely bloated
    /// </summary>
    [DataField("stage"), ViewVariables(VVAccess.ReadWrite)]
    public int Stage;
}