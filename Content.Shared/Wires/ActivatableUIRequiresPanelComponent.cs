// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aidenkrz <aiden@djkraz.com>
// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Robust.Shared.GameStates;

namespace Content.Shared.Wires;

/// <summary>
/// This is used for activatable UIs that require the entity to have a panel in a certain state.
/// </summary>
[RegisterComponent, NetworkedComponent, Access(typeof(SharedWiresSystem))]
public sealed partial class ActivatableUIRequiresPanelComponent : Component
{
    /// <summary>
    /// TRUE: the panel must be open to access the UI.
    /// FALSE: the panel must be closed to access the UI.
    /// </summary>
    [DataField]
    public bool RequireOpen = true;
}