// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

namespace Content.Shared.Movement.Pulling.Components;

/// <summary>
/// Component that indicates that an entity is currently pulling some other entity.
/// </summary>
[RegisterComponent]
public sealed partial class ActivePullerComponent : Component;