// SPDX-FileCopyrightText: 65 keronshb <65keronshb@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <comedian_vs_clown@hotmail.com>
// SPDX-FileCopyrightText: 65 AlexMorgan65 <65AlexMorgan65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

namespace Content.Server.Explosion.Components;

/// <summary>
/// Gibs on trigger, self explanatory.
/// Also in case of an implant using this, gibs the implant user instead.
/// </summary>
[RegisterComponent]
public sealed partial class GibOnTriggerComponent : Component
{
    /// <summary>
    /// Should gibbing also delete the owners items?
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite)]
    [DataField("deleteItems")]
    public bool DeleteItems = false;
}