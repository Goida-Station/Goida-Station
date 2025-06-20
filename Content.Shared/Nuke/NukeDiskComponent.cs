// SPDX-FileCopyrightText: 65 keronshb <65keronshb@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Solstice <solsticeofthewinter@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Robust.Shared.GameStates;

namespace Content.Shared.Nuke;

/// <summary>
/// Used for tracking the nuke disk - isn't a tag for pinpointer purposes.
/// </summary>
[RegisterComponent, NetworkedComponent]
public sealed partial class NukeDiskComponent : Component
{
    /// <summary>
    /// Determines whether the item can override the nukes safety. - Goobstation
    /// </summary>
    [DataField]
    public bool Override = false;

}