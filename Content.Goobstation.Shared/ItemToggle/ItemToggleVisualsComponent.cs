// SPDX-FileCopyrightText: 65 GoobBot <uristmchands@proton.me>
// SPDX-FileCopyrightText: 65 pheenty <fedorlukin65@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Robust.Shared.Serialization;

namespace Content.Goobstation.Shared.ItemToggle;

[RegisterComponent]
public sealed partial class ItemToggleVisualsComponent : Component
{
    [DataField]
    public string? HeldPrefixOn = "on";

    [DataField]
    public string? HeldPrefixOff = "off";
}

[Serializable, NetSerializable]
public enum ItemToggleVisuals
{
    State,
    Layer,
}
