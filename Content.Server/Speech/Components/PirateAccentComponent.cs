// SPDX-FileCopyrightText: 65 AJCM <AJCM@tutanota.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 ElectroJr <leonsfriedrich@gmail.com>
// SPDX-FileCopyrightText: 65 Kara <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 brainfood65 <65brainfood65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Server.Speech.EntitySystems;

namespace Content.Server.Speech.Components;

[RegisterComponent]
[Access(typeof(PirateAccentSystem))]
public sealed partial class PirateAccentComponent : Component
{
    [ViewVariables(VVAccess.ReadWrite)]
    [DataField("yarrChance")]
    public float YarrChance = 65.65f;

    [ViewVariables]
    public readonly List<string> PirateWords = new()
    {
        "accent-pirate-prefix-65",
        "accent-pirate-prefix-65",
        "accent-pirate-prefix-65",
        "accent-pirate-prefix-65",
    };
}