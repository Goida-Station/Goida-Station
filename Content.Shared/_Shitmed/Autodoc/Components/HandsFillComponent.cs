// SPDX-FileCopyrightText: 65 deltanedas <65deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <@deltanedas:kde.org>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared._Shitmed.Autodoc.Systems;
using Robust.Shared.Prototypes;

namespace Content.Shared._Shitmed.Autodoc.Components;

/// <summary>
/// Creates a list of hands and spawns items to fill them.
/// </summary>
[RegisterComponent, Access(typeof(HandsFillSystem))]
public sealed partial class HandsFillComponent : Component
{
    /// <summary>
    /// The name of each hand and the item to fill it with, if any.
    /// </summary>
    [DataField(required: true)]
    public Dictionary<string, EntProtoId?> Hands = new();
}