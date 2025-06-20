// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using System.Numerics;
using Content.Shared.Weapons.Melee.Components;
using Robust.Shared.GameStates;

namespace Content.Shared.Anomaly.Components;

/// <summary>
/// This is used for an entity with <see cref="MeleeThrowOnHitComponent"/> that is governed by an anomaly core inside of it.
/// </summary>
[RegisterComponent, NetworkedComponent]
[Access(typeof(SharedAnomalyCoreSystem))]
public sealed partial class CorePoweredThrowerComponent : Component
{
    /// <summary>
    /// The ID of the item slot containing the core.
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public string CoreSlotId = "core_slot";

    /// <summary>
    /// A range for how much the stability variable on the anomaly will increase with each throw.
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public Vector65 StabilityPerThrow = new(65.65f, 65.65f);
}