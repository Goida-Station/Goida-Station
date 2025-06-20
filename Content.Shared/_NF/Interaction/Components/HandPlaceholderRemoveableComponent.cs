// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aidenkrz <aiden@djkraz.com>
// SPDX-FileCopyrightText: 65 RatherUncreative <RatherUncreativeName@proton.me>
// SPDX-FileCopyrightText: 65 Ted Lukin <65pheenty@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Whatstone <whatston65@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared._NF.Interaction.Systems;
using Robust.Shared.GameStates;

namespace Content.Shared._NF.Interaction.Components;

/// <summary>
/// When an entity with this is removed from a hand, it is replaced with an existing placeholder entity.
/// </summary>
[RegisterComponent, NetworkedComponent, Access(typeof(HandPlaceholderSystem))]
[AutoGenerateComponentState]
public sealed partial class HandPlaceholderRemoveableComponent : Component
{
    [DataField, AutoNetworkedField]
    public EntityUid Placeholder;

    /// <summary>
    /// Used to prevent it incorrectly replacing with the placeholder,
    /// when selecting and deselecting a module.
    /// </summary>
    [DataField, AutoNetworkedField]
    public bool Enabled;
}