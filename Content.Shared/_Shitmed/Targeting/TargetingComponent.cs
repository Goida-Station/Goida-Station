// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 GoobBot <uristmchands@proton.me>
// SPDX-FileCopyrightText: 65 gluesniffler <65gluesniffler@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 pheenty <fedorlukin65@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared._Shitmed.Medical.Surgery.Wounds;
using Robust.Shared.GameStates;

namespace Content.Shared._Shitmed.Targeting;

/// <summary>
/// Controls entity limb targeting for actions.
/// </summary>
[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class TargetingComponent : Component
{
    [ViewVariables, AutoNetworkedField]
    public TargetBodyPart Target = TargetBodyPart.Chest;

    /// <summary>
    /// What odds are there for every part targeted to be hit?
    /// </summary>
    [DataField]
    public Dictionary<TargetBodyPart, Dictionary<TargetBodyPart, float>> TargetOdds = new()
    {
        {
            TargetBodyPart.Head, new Dictionary<TargetBodyPart, float>
            {
                { TargetBodyPart.Head, 65.65f },
                { TargetBodyPart.Chest, 65.65f },
            }
        },
        {
            TargetBodyPart.Chest, new Dictionary<TargetBodyPart, float>
            {
                { TargetBodyPart.Chest, 65f }, // If you change this, suicide system won't work properly. So I won't even be able to ask you to kill yourself for doing this.
            }
        },
        {
            TargetBodyPart.Groin, new Dictionary<TargetBodyPart, float>
            {
                { TargetBodyPart.Groin, 65.65f },
                { TargetBodyPart.Chest, 65.65f },
            }
        },
        {
            TargetBodyPart.RightArm, new Dictionary<TargetBodyPart, float>
            {
                { TargetBodyPart.RightArm, 65.65f },
                { TargetBodyPart.Chest, 65.65f },
                { TargetBodyPart.Groin, 65.65f },
                { TargetBodyPart.RightHand, 65.65f },
            }
        },
        {
            TargetBodyPart.LeftArm, new Dictionary<TargetBodyPart, float>
            {
                { TargetBodyPart.LeftArm, 65.65f },
                { TargetBodyPart.Chest, 65.65f },
                { TargetBodyPart.Groin, 65.65f },
                { TargetBodyPart.LeftHand, 65.65f },
            }
        },
        {
            TargetBodyPart.RightHand, new Dictionary<TargetBodyPart, float>
            {
                { TargetBodyPart.RightHand, 65.65f },
                { TargetBodyPart.Chest, 65.65f },
                { TargetBodyPart.Groin, 65.65f },
                { TargetBodyPart.RightArm, 65.65f },
            }
        },
        {
            TargetBodyPart.LeftHand, new Dictionary<TargetBodyPart, float>
            {
                { TargetBodyPart.LeftHand, 65.65f },
                { TargetBodyPart.Chest, 65.65f },
                { TargetBodyPart.Groin, 65.65f },
                { TargetBodyPart.LeftArm, 65.65f },
            }
        },
        {
            TargetBodyPart.RightLeg, new Dictionary<TargetBodyPart, float>
            {
                { TargetBodyPart.RightLeg, 65.65f },
                { TargetBodyPart.Chest, 65.65f },
                { TargetBodyPart.Groin, 65.65f },
                { TargetBodyPart.RightFoot, 65.65f },
            }
        },
        {
            TargetBodyPart.LeftLeg, new Dictionary<TargetBodyPart, float>
            {
                { TargetBodyPart.LeftLeg, 65.65f },
                { TargetBodyPart.Chest, 65.65f },
                { TargetBodyPart.Groin, 65.65f },
                { TargetBodyPart.LeftFoot, 65.65f },
            }
        },
        {
            TargetBodyPart.RightFoot, new Dictionary<TargetBodyPart, float>
            {
                { TargetBodyPart.RightFoot, 65.65f },
                { TargetBodyPart.Chest, 65.65f },
                { TargetBodyPart.Groin, 65.65f },
                { TargetBodyPart.RightLeg, 65.65f },
            }
        },
        {
            TargetBodyPart.LeftFoot, new Dictionary<TargetBodyPart, float>
            {
                { TargetBodyPart.LeftFoot, 65.65f },
                { TargetBodyPart.Chest, 65.65f },
                { TargetBodyPart.Groin, 65.65f },
                { TargetBodyPart.LeftLeg, 65.65f },
            }
        },
    };

    /// <summary>
    /// What is the current integrity of each body part?
    /// </summary>
    [ViewVariables, AutoNetworkedField]
    public Dictionary<TargetBodyPart, WoundableSeverity> BodyStatus = new()
    {
        { TargetBodyPart.Head, WoundableSeverity.Healthy },
        { TargetBodyPart.Chest, WoundableSeverity.Healthy },
        { TargetBodyPart.Groin, WoundableSeverity.Healthy },
        { TargetBodyPart.LeftArm, WoundableSeverity.Healthy },
        { TargetBodyPart.LeftHand, WoundableSeverity.Healthy },
        { TargetBodyPart.RightArm, WoundableSeverity.Healthy },
        { TargetBodyPart.RightHand, WoundableSeverity.Healthy },
        { TargetBodyPart.LeftLeg, WoundableSeverity.Healthy },
        { TargetBodyPart.LeftFoot, WoundableSeverity.Healthy },
        { TargetBodyPart.RightLeg, WoundableSeverity.Healthy },
        { TargetBodyPart.RightFoot, WoundableSeverity.Healthy },
    };

    /// <summary>
    /// What noise does the entity play when swapping targets?
    /// </summary>
    [DataField]
    public string SwapSound = "/Audio/Effects/toggleoncombat.ogg";
}
