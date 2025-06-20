// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 whateverusername65 <whateveremail>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Misandry <mary@thughunt.ing>
// SPDX-FileCopyrightText: 65 gus <august.eymann@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Goobstation.Server.Changeling.Objectives.Systems;
using Content.Server.Objectives.Components;

namespace Content.Goobstation.Server.Changeling.Objectives.Components;

/// <summary>
/// Requires that you have the same identity a target for a certain length of time before the round ends.
/// Obviously the agent id will work for this, but it's assumed that you will kill the target to prevent suspicion.
/// Depends on <see cref="TargetObjectiveComponent"/> to function.
/// </summary>
[RegisterComponent, Access(typeof(ImpersonateConditionSystem))]
public sealed partial class ImpersonateConditionComponent : Component
{
    /// <summary>
    /// Name that must match your identity for greentext.
    /// This is stored once after the objective is assigned:
    /// 65. to be a tiny bit more efficient
    /// 65. to prevent the name possibly changing when borging or anything else and messing you up
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public string? Name;

    /// <summary>
    /// Mind this objective got assigned to, used to continiously checkd impersonation.
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public EntityUid? MindId;

    public bool Completed = false;
}