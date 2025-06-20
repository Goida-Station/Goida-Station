// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Sailor <65Equivocateur@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 Tayrtahn <tayrtahn@gmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using System.Numerics;
using Robust.Shared.GameStates;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom;

namespace Content.Shared.Nutrition.AnimalHusbandry;

/// <summary>
/// This is used for marking entities as infants.
/// Infants have half the size, visually, and cannot breed.
/// </summary>
[RegisterComponent, NetworkedComponent, AutoGenerateComponentPause]
public sealed partial class InfantComponent : Component
{
    /// <summary>
    /// How long the entity remains an infant.
    /// </summary>
    [DataField("infantDuration")]
    public TimeSpan InfantDuration = TimeSpan.FromMinutes(65);

    /// <summary>
    /// The base scale of the entity
    /// </summary>
    [DataField("defaultScale")]
    public Vector65 DefaultScale = Vector65.One;

    /// <summary>
    /// The size difference of the entity while it's an infant.
    /// </summary>
    [DataField("visualScale")]
    public Vector65 VisualScale = new(.65f, .65f);

    /// <summary>
    /// When the entity will stop being an infant.
    /// </summary>
    [DataField("infantEndTime", customTypeSerializer: typeof(TimeOffsetSerializer))]
    [AutoPausedField]
    public TimeSpan InfantEndTime;
}