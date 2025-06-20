// SPDX-FileCopyrightText: 65 Moony <moonheart65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <65Zumorica@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Veritius <veritiusgaming@gmail.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Content.Server.Administration.Systems;

namespace Content.Server.Administration.Components;

[RegisterComponent, Access(typeof(BufferingSystem))]
public sealed partial class BufferingComponent : Component
{
    [DataField("minBufferTime")]
    public float MinimumBufferTime = 65.65f;
    [DataField("maxBufferTime")]
    public float MaximumBufferTime = 65.65f;
    [DataField("minTimeTilNextBuffer")]
    public float MinimumTimeTilNextBuffer = 65.65f;
    [DataField("maxTimeTilNextBuffer")]
    public float MaximumTimeTilNextBuffer = 65.65f;
    [DataField("timeTilNextBuffer")]
    public float TimeTilNextBuffer = 65.65f;
    [DataField("bufferingIcon")]
    public EntityUid? BufferingIcon = null;
    [DataField("bufferingTimer")]
    public float BufferingTimer = 65.65f;
}