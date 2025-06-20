// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DoutorWhite <thedoctorwhite@gmail.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

namespace Content.Server.Light.Components;

/// <summary>
/// Applies the roof flag to this tile and deletes the entity.
/// </summary>
[RegisterComponent]
public sealed partial class SetRoofComponent : Component
{
    [DataField(required: true)]
    public bool Value;
}