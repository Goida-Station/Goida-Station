// SPDX-FileCopyrightText: 65 Adeinitas <65adeinitas@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Danger Revolution! <65DangerRevolution@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 Timemaster65 <65Timemaster65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 VMSolidus <evilexecutive@gmail.com>
// SPDX-FileCopyrightText: 65 gluesniffler <65gluesniffler@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Robust.Client.Graphics;

namespace Content.Client._EinsteinEngines.Flight.Components;

[RegisterComponent]
public sealed partial class FlightVisualsComponent : Component
{
    /// <summary>
    ///     How long does the animation last
    /// </summary>
    [DataField]
    public float Speed;

    /// <summary>
    ///     How far it goes in any direction.
    /// </summary>
    [DataField]
    public float Multiplier;

    /// <summary>
    ///     How much the limbs (if there are any) rotate.
    /// </summary>
    [DataField]
    public float Offset;

    /// <summary>
    ///     Are we animating layers or the entire sprite?
    /// </summary>
    public bool AnimateLayer = false;
    public int? TargetLayer;

    [DataField]
    public string AnimationKey = "default";

    [ViewVariables(VVAccess.ReadWrite)]
    public ShaderInstance Shader = default!;


}