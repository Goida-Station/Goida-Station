// SPDX-FileCopyrightText: 65 Flipp Syder <65vulppine@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

namespace Content.Server.Nutrition.Components;

[RegisterComponent]
public sealed partial class FlavorProfileComponent : Component
{
    /// <summary>
    ///     Localized string containing the base flavor of this entity.
    /// </summary>
    [DataField("flavors")]
    public HashSet<string> Flavors { get; private set; } = new();

    /// <summary>
    ///     Reagent IDs to ignore when processing this flavor profile. Defaults to nutriment.
    /// </summary>
    [DataField("ignoreReagents")]
    public HashSet<string> IgnoreReagents { get; private set; } = new()
    {
        "Nutriment",
        "Vitamin",
        "Protein"
    };
}