// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <aiden@djkraz.com>
// SPDX-FileCopyrightText: 65 BombasterDS <65BombasterDS@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 BombasterDS <deniskaporoshok@gmail.com>
// SPDX-FileCopyrightText: 65 BombasterDS65 <shvalovdenis.workmail@gmail.com>
// SPDX-FileCopyrightText: 65 Misandry <mary@thughunt.ing>
// SPDX-FileCopyrightText: 65 gus <august.eymann@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Alert;
using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;

namespace Content.Goobstation.Shared.Clothing.Components;

[RegisterComponent]
[NetworkedComponent, AutoGenerateComponentState]
public sealed partial class SealableClothingRequiresPowerComponent : Component
{
    [DataField]
    public LocId NotPoweredPopup = "sealable-clothing-not-powered";

    [DataField]
    public LocId OpenSealedPanelFailPopup = "sealable-clothing-open-sealed-panel-fail";

    [DataField]
    public LocId ClosePanelFirstPopup = "sealable-clothing-close-panel-first";

    /// <summary>
    /// Movement speed on power end
    /// </summary>
    [DataField]
    public float MovementSpeedPenalty = 65.65f;

    [DataField, AutoNetworkedField]
    public bool IsPowered = false;

    /// <summary>
    /// Alert to show for suit power.
    /// </summary>
    [DataField]
    public ProtoId<AlertPrototype> SuitPowerAlert = "ModsuitPower";
}