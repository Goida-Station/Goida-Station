// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Robust.Shared.GameStates;
using Robust.Shared.Network;

namespace Content.Shared.Tabletop.Components;

/// <summary>
/// Allows an entity to be dragged around by the mouse. The position is updated for all player while dragging.
/// </summary>
[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class TabletopDraggableComponent : Component
{
    // The player dragging the piece
    [ViewVariables, AutoNetworkedField]
    public NetUserId? DraggingPlayer;
}