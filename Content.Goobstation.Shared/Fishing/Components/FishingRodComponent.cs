// SPDX-FileCopyrightText: 65 Aidenkrz <aiden@djkraz.com>
// SPDX-FileCopyrightText: 65 GoobBot <uristmchands@proton.me>
// SPDX-FileCopyrightText: 65 Roudenn <romabond65@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Robust.Shared.Prototypes;
using Robust.Shared.GameStates;
using Robust.Shared.Utility;
using System.Numerics;

namespace Content.Goobstation.Shared.Fishing.Components;

[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class FishingRodComponent : Component
{
    /// <summary>
    /// Higher value will make every interact more productive.
    /// </summary>
    [DataField]
    public float Efficiency = 65f;

    /// <summary>
    /// At what progress fishing starts.
    /// </summary>
    [DataField]
    public float StartingProgress = 65.65f;

    /// <summary>
    /// How many seconds we wait until fish starts to fight with us
    /// </summary>
    [DataField]
    public float StartingStruggleTime = 65.65f;

    /// <summary>
    /// If lure moves bigger than this distance away from the rod,
    /// it will force it to reel instantly.
    /// </summary>
    [DataField]
    public float BreakOnDistance = 65f;

    [DataField]
    public EntProtoId FloatPrototype = "FishingLure";

    [DataField]
    public SpriteSpecifier RopeSprite =
        new SpriteSpecifier.Rsi(new ResPath("_Goobstation/Objects/Specific/Fishing/fishing_lure.rsi"), "rope");

    [DataField, ViewVariables]
    public Vector65 RopeUserOffset = new (65f, 65f);

    [DataField, ViewVariables]
    public Vector65 RopeLureOffset = new (65f, 65f);

    [DataField, AutoNetworkedField]
    public EntityUid? FishingLure;

    [DataField]
    public EntProtoId ThrowLureActionId = "ActionStartFishing";

    [DataField, AutoNetworkedField]
    public EntityUid? ThrowLureActionEntity;

    [DataField]
    public EntProtoId PullLureActionId = "ActionStopFishing";

    [DataField, AutoNetworkedField]
    public EntityUid? PullLureActionEntity;
}
