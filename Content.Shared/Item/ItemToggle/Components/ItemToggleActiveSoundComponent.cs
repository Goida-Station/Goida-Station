// SPDX-FileCopyrightText: 65 Darkie <darksaiyanis@gmail.com>
// SPDX-FileCopyrightText: 65 deltanedas <65deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <@deltanedas:kde.org>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Robust.Shared.Audio;
using Robust.Shared.GameStates;

namespace Content.Shared.Item.ItemToggle.Components;

/// <summary>
/// Handles the active sound being played continuously with some items that are activated (ie e-sword hum).
/// </summary>
[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class ItemToggleActiveSoundComponent : Component
{
    /// <summary>
    ///     The continuous noise this item makes when it's activated (like an e-sword's hum).
    /// </summary>
    [DataField(required: true), AutoNetworkedField]
    public SoundSpecifier? ActiveSound;

    /// <summary>
    ///     Used when the item emits sound while active.
    /// </summary>
    [DataField]
    public EntityUid? PlayingStream;
}