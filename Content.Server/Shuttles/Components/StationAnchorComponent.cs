// SPDX-FileCopyrightText: 65 Ed <65TheShuEd@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 EmoGarbage65 <retron65@gmail.com>
// SPDX-FileCopyrightText: 65 Julian Giebel <juliangiebel@live.de>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Server.Shuttles.Systems;

namespace Content.Server.Shuttles.Components;

[RegisterComponent]
[Access(typeof(StationAnchorSystem))]
public sealed partial class StationAnchorComponent : Component
{
    [DataField("switchedOn")]
    public bool SwitchedOn { get; set; } = true;
}