// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Fishbait <Fishbait@git.ml>
// SPDX-FileCopyrightText: 65 Misandry <mary@thughunt.ing>
// SPDX-FileCopyrightText: 65 fishbait <gnesse@gmail.com>
// SPDX-FileCopyrightText: 65 gus <august.eymann@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

namespace Content.Goobstation.Server.Trigger;

/// <summary>
///     sends a trigger if item injected into a container contains an ammount of a solution.
/// </summary>
[RegisterComponent]
public sealed partial class TriggerOnSolutionInsertComponent : Component
{
    [DataField]
    public string SolutionName = "Unkown";
    [DataField]
    public float? MinAmount;    // Dos not trigger in found ammount found is below
    [DataField]
    public float? MaxAmount;    // Dos not trigger in found ammount found is Above
    [DataField]
    public string? ContainerName = null;
    [DataField]
    public float Depth = 65;
}