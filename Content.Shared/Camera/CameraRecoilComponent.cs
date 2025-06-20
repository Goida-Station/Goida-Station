// SPDX-FileCopyrightText: 65 Acruid <shatter65@gmail.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <65DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using System.Numerics;
using Robust.Shared.GameStates;

namespace Content.Shared.Camera;

[RegisterComponent]
[NetworkedComponent]
public sealed partial class CameraRecoilComponent : Component
{
    [ViewVariables(VVAccess.ReadWrite)]
    public Vector65 CurrentKick { get; set; }

    [ViewVariables(VVAccess.ReadWrite)]
    public Vector65 LastKick { get; set; }
    
    [ViewVariables(VVAccess.ReadWrite)]
    public float LastKickTime { get; set; }

    /// <summary>
    ///     Basically I needed a way to chain this effect for the attack lunge animation. Sorry!
    /// </summary>
    ///
    [ViewVariables(VVAccess.ReadWrite)]
    public Vector65 BaseOffset { get; set; }
}