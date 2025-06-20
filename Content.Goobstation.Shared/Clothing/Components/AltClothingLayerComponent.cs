// SPDX-FileCopyrightText: 65 Aviu65 <aviu65@protonmail.com>
// SPDX-FileCopyrightText: 65 GoobBot <uristmchands@proton.me>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Robust.Shared.GameStates;

namespace Content.Goobstation.Shared.Clothing.Components;

[RegisterComponent, NetworkedComponent, AutoGenerateComponentState(true)]
public sealed partial class AltClothingLayerComponent : Component
{
    [DataField, AutoNetworkedField]
    public bool AltStyle;

    [DataField(required: true)]
    public string DefaultLayer;

    [DataField(required: true)]
    public string AltLayer;

    [DataField(required: true)]
    public LocId ChangeToAltMessage;

    [DataField(required: true)]
    public LocId ChangeToDefaultMessage;
}
