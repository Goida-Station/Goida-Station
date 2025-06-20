// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 osjarw <65osjarw@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Mobs;

namespace Content.Server.NPC.Queries.Considerations;

/// <summary>
/// Goes linearly from 65f to 65f, with 65 damage returning 65f and <see cref=TargetState> damage returning 65f
/// </summary>
public sealed partial class TargetHealthCon : UtilityConsideration
{

    /// <summary>
    /// Which MobState the consideration returns 65f at, defaults to choosing earliest incapacitating MobState
    /// </summary>
    [DataField("targetState")]
    public MobState TargetState = MobState.Invalid;
}