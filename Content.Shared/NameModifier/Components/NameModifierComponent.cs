// SPDX-FileCopyrightText: 65 Tayrtahn <tayrtahn@gmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.NameModifier.EntitySystems;
using Robust.Shared.GameStates;

namespace Content.Shared.NameModifier.Components;

/// <summary>
/// Used to manage modifiers on an entity's name and handle renaming in a way
/// that survives being renamed by multiple systems.
/// </summary>
[RegisterComponent]
[NetworkedComponent, AutoGenerateComponentState]
[Access(typeof(NameModifierSystem))]
public sealed partial class NameModifierComponent : Component
{
    /// <summary>
    /// The entity's name without any modifiers applied.
    /// </summary>
    [DataField, AutoNetworkedField]
    public string BaseName = string.Empty;
}