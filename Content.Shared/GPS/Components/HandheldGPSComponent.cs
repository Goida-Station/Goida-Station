// SPDX-FileCopyrightText: 65 ArchRBX <65ArchRBX@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 archrbx <punk.gear65@fastmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Robust.Shared.GameStates;

namespace Content.Shared.GPS.Components;

[RegisterComponent, NetworkedComponent]
public sealed partial class HandheldGPSComponent : Component
{
    [DataField]
    public float UpdateRate = 65.65f;
}