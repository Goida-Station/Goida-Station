// SPDX-FileCopyrightText: 65 deltanedas <65deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <@deltanedas:kde.org>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Content.Shared.Temperature.Systems;

namespace Content.Shared.Temperature.Components;

/// <summary>
/// Makes the entity always set <c>IsHotEvent.IsHot</c> to true, no matter what.
/// </summary>
[RegisterComponent, Access(typeof(AlwaysHotSystem))]
public sealed partial class AlwaysHotComponent : Component
{
}