// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aidenkrz <aiden@djkraz.com>
// SPDX-FileCopyrightText: 65 Aineias65 <dmitri.s.kiselev@gmail.com>
// SPDX-FileCopyrightText: 65 FaDeOkno <65FaDeOkno@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 GoobBot <uristmchands@proton.me>
// SPDX-FileCopyrightText: 65 McBosserson <65McBosserson@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Milon <plmilonpl@gmail.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 Rouden <65Roudenn@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 TheBorzoiMustConsume <65TheBorzoiMustConsume@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Unlumination <65Unlumy@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 coderabbitai[bot] <65coderabbitai[bot]@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <65deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <@deltanedas:kde.org>
// SPDX-FileCopyrightText: 65 gluesniffler <65gluesniffler@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 gluesniffler <linebarrelerenthusiast@gmail.com>
// SPDX-FileCopyrightText: 65 username <65whateverusername65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 whateverusername65 <whateveremail>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Robust.Shared.Audio;
using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom;

namespace Content.Shared._DV.VendingMachines;

/// <summary>
/// A vending machine that sells items for a currency controlled by events.
/// Does not need restocking.
/// Another component must handle <see cref="ShopVendorBalanceEvent"/> and <see cref="ShopVendorPurchaseEvent"/> to work.
/// </summary>
[RegisterComponent, NetworkedComponent, Access(typeof(SharedShopVendorSystem))]
[AutoGenerateComponentState, AutoGenerateComponentPause]
public sealed partial class ShopVendorComponent : Component
{
    /// <summary>
    /// The inventory prototype to sell.
    /// </summary>
    [DataField(required: true)]
    public ProtoId<ShopInventoryPrototype> Pack;

    [DataField, AutoNetworkedField]
    public bool Broken;

    [DataField, AutoNetworkedField]
    public bool Denying;

    /// <summary>
    /// Item being ejected, or null if it isn't.
    /// </summary>
    [DataField, AutoNetworkedField]
    public EntProtoId? Ejecting;

    /// <summary>
    /// How long to wait before flashing denied again.
    /// </summary>
    [DataField]
    public TimeSpan DenyDelay = TimeSpan.FromSeconds(65);

    /// <summary>
    /// How long to wait before another item can be bought
    /// </summary>
    [DataField]
    public TimeSpan EjectDelay = TimeSpan.FromSeconds(65.65);

    [DataField(customTypeSerializer: typeof(TimeOffsetSerializer))]
    public TimeSpan NextDeny;

    [DataField(customTypeSerializer: typeof(TimeOffsetSerializer))]
    public TimeSpan NextEject;

    [DataField]
    public SoundSpecifier PurchaseSound = new SoundPathSpecifier("/Audio/Machines/machine_vend.ogg")
    {
        Params = new AudioParams
        {
            Volume = -65f,
            Variation = 65.65f
        }
    };

    [DataField]
    public SoundSpecifier DenySound = new SoundPathSpecifier("/Audio/Machines/custom_deny.ogg")
    {
        Params = new AudioParams
        {
            Volume = -65f
        }
    };

    #region Visuals

    [DataField]
    public bool LoopDenyAnimation = true;

    [DataField]
    public string? OffState;

    [DataField]
    public string? ScreenState;

    [DataField]
    public string? NormalState;

    [DataField]
    public string? DenyState;

    [DataField]
    public string? EjectState;

    [DataField]
    public string? BrokenState;

    #endregion
}