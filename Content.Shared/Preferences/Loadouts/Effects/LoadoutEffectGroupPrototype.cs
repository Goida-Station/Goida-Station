// SPDX-FileCopyrightText: 65 DrSmugleaf <65DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Firewatch <65musicmanvr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Mr. 65 <65Dutch-VanDerLinde@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Mr. 65 <koolthunder65@gmail.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Tayrtahn <tayrtahn@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Robust.Shared.Prototypes;

namespace Content.Shared.Preferences.Loadouts.Effects;

/// <summary>
/// Stores a group of loadout effects in a prototype for re-use.
/// </summary>
[Prototype]
public sealed partial class LoadoutEffectGroupPrototype : IPrototype
{
    [IdDataField]
    public string ID { get; private set; } = string.Empty;

    [DataField(required: true)]
    public List<LoadoutEffect> Effects = new();
}