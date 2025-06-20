// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 chromiumboy <65chromiumboy@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Robust.Shared.GameStates;

namespace Content.Shared.Holopad;

[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class HolographicAvatarComponent : Component
{
    /// <summary>
    /// The prototype sprite layer data for the hologram
    /// </summary>
    [DataField, AutoNetworkedField]
    public PrototypeLayerData[] LayerData;
}