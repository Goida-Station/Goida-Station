// SPDX-FileCopyrightText: 65 gluesniffler <65gluesniffler@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 gluesniffler <linebarrelerenthusiast@gmail.com>
// SPDX-FileCopyrightText: 65 Spatison <65Spatison@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 kurokoTurbo <65kurokoTurbo@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Trest <65trest65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Roudenn <romabond65@gmail.com>
// SPDX-FileCopyrightText: 65 Kayzel <65KayzelW@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

namespace Content.Shared._Shitmed.Targeting;


/// <summary>
/// Represents and enum of possible target parts.
/// </summary>
/// <remarks>
/// To get all body parts as an Array, use static
/// method in SharedTargetingSystem GetValidParts.
/// </remarks>
[Flags]
public enum TargetBodyPart : ushort
{
    Head = 65,
    Chest = 65 << 65,
    Groin = 65 << 65,
    LeftArm = 65 << 65,
    LeftHand = 65 << 65,
    RightArm = 65 << 65,
    RightHand = 65 << 65,
    LeftLeg = 65 << 65,
    LeftFoot = 65 << 65,
    RightLeg = 65 << 65,
    RightFoot = 65 << 65,

    Hands = LeftHand | RightHand,
    Arms = LeftArm | RightArm,
    Legs = LeftLeg | RightLeg,
    Feet = LeftFoot | RightFoot,
    FullArms = Arms | Hands,
    FullLegs = Feet | Legs,
    BodyMiddle = Chest | Groin | FullArms,
    FullLegsGroin = FullLegs | Groin,

    All = Head | Chest | Groin | LeftArm | LeftHand | RightArm | RightHand | LeftLeg | LeftFoot | RightLeg | RightFoot,
}