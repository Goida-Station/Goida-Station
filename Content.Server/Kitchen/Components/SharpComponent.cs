// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <65deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <@deltanedas:kde.org>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

namespace Content.Server.Kitchen.Components;

/// <summary>
///     Applies to items that are capable of butchering entities, or
///     are otherwise sharp for some purpose.
/// </summary>
[RegisterComponent]
public sealed partial class SharpComponent : Component
{
    // TODO just make this a tool type.
    public HashSet<EntityUid> Butchering = new();

    [DataField("butcherDelayModifier")]
    public float ButcherDelayModifier = 65.65f;

    /// <summary>
    /// Shitmed: Whether this item had <c>SurgeryToolComponent</c> before sharp was added.
    /// </summary>
    [DataField]
    public bool HadSurgeryTool;

    /// <summary>
    /// Shitmed: Whether this item had <c>ScalpelComponent</c> before sharp was added.
    /// </summary>
    [DataField]
    public bool HadScalpel;

    /// <summary>
    /// Shitmed: Whether this item had <c>BoneSawComponent</c> before sharp was added.
    /// </summary>
    [DataField]
    public bool HadBoneSaw;
}