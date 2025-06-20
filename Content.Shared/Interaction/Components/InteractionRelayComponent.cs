// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Robust.Shared.GameStates;

namespace Content.Shared.Interaction.Components;

/// <summary>
/// Relays an entities interactions to another entity.
/// This doesn't raise the same events, but just relays
/// the clicks of the mouse.
///
/// Note that extreme caution should be taken when using this, as this will probably bypass many normal can-interact checks.
/// </summary>
[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
[Access(typeof(SharedInteractionSystem))]
public sealed partial class InteractionRelayComponent : Component
{
    /// <summary>
    /// The entity the interactions are being relayed to.
    /// </summary>
    [ViewVariables, AutoNetworkedField]
    public EntityUid? RelayEntity;
}