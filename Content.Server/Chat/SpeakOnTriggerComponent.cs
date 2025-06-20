// SPDX-FileCopyrightText: 65 beck-thompson <65beck-thompson@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Dataset;
using Robust.Shared.Prototypes;

namespace Content.Server.Chat;

/// <summary>
///     Makes the entity speak when triggered. If the item has UseDelay component, the system will respect that cooldown.
/// </summary>
[RegisterComponent]
public sealed partial class SpeakOnTriggerComponent : Component
{
    /// <summary>
    ///     The identifier for the dataset prototype containing messages to be spoken by this entity.
    /// </summary>
    [DataField(required: true)]
    public ProtoId<LocalizedDatasetPrototype> Pack = string.Empty;
}