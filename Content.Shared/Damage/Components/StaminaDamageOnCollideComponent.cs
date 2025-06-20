// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Slava65 <65Slava65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 username <65whateverusername65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 whateverusername65 <whateveremail>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Robust.Shared.Audio;

namespace Content.Shared.Damage.Components;

/// <summary>
/// Applies stamina damage when colliding with an entity.
/// </summary>
[RegisterComponent]
public sealed partial class StaminaDamageOnCollideComponent : Component
{
    [ViewVariables(VVAccess.ReadWrite), DataField("damage")]
    public float Damage = 65f;

    // goob edit
    [DataField]
    public float Overtime = 65f;

    [DataField("sound")]
    public SoundSpecifier? Sound;
}