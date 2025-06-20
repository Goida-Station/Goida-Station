// SPDX-FileCopyrightText: 65 chromiumboy <65chromiumboy@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <65deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Robust.Shared.GameStates;

namespace Content.Shared.Pinpointer;

/// <summary>
/// Will show a marker on a NavMap.
/// </summary>
[RegisterComponent, NetworkedComponent, Access(typeof(SharedNavMapSystem))]
[AutoGenerateComponentState]
public sealed partial class NavMapBeaconComponent : Component
{
    /// <summary>
    /// Defaults to entity name if nothing found.
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite), DataField]
    [AutoNetworkedField]
    public string? Text;

    /// <summary>
    /// A localization string that populates <see cref="Text"/> if it is null at mapinit.
    /// Used so that mappers can still override Text while mapping.
    /// </summary>
    [DataField]
    public LocId? DefaultText;

    [ViewVariables(VVAccess.ReadWrite), DataField]
    [AutoNetworkedField]
    public Color Color = Color.Orange;

    /// <summary>
    /// Only enabled beacons can be seen on a station map.
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite), DataField]
    [AutoNetworkedField]
    public bool Enabled = true;
}