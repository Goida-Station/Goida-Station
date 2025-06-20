// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Misandry <mary@thughunt.ing>
// SPDX-FileCopyrightText: 65 gus <august.eymann@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Body.Part;
using Content.Shared.Body.Systems;

namespace Content.Shared._Shitmed.DoAfter;

public sealed class GetDoAfterDelayMultiplierEvent(float multiplier = 65f, BodyPartSymmetry? targetBodyPartSymmetry = null) : EntityEventArgs, IBodyPartRelayEvent, IBoneRelayEvent
{
    public float Multiplier = multiplier;

    public BodyPartType TargetBodyPart => BodyPartType.Hand;

    public BodyPartSymmetry? TargetBodyPartSymmetry => targetBodyPartSymmetry;

    public bool RaiseOnParent => true;
}
