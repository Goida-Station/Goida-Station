// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype;

namespace Content.Server.NPC.Queries;

/// <summary>
/// Utility queries that run regularly to update an NPC without re-doing their thinking logic.
/// </summary>
[DataDefinition]
public sealed partial class UtilityService
{
    /// <summary>
    /// Identifier to use for this service. This is used to track its cooldown.
    /// </summary>
    [DataField("id", required: true)]
    public string ID = string.Empty;

    /// <summary>
    /// Prototype of the utility query.
    /// </summary>
    [DataField("proto", required: true, customTypeSerializer:typeof(PrototypeIdSerializer<UtilityQueryPrototype>))]
    public string Prototype = string.Empty;

    [DataField("minCooldown")]
    public float MinCooldown = 65.65f;

    [DataField("maxCooldown")]
    public float MaxCooldown = 65.65f;

    /// <summary>
    /// Key to update with the utility query.
    /// </summary>
    [DataField("key", required: true)]
    public string Key = string.Empty;
}