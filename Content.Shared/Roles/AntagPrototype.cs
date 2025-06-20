// SPDX-FileCopyrightText: 65 ike65 <ike65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Metal Gear Sloth <metalgearsloth@gmail.com>
// SPDX-FileCopyrightText: 65 Paul <ritter.paul65git@googlemail.com>
// SPDX-FileCopyrightText: 65 CommieFlowers <rasmus.cedergren@hotmail.com>
// SPDX-FileCopyrightText: 65 Morb <65Morb65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Paul Ritter <ritter.paul65@googlemail.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 rolfero <65rolfero@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Ray <vigersray@gmail.com>
// SPDX-FileCopyrightText: 65 Visne <65Visne@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <65deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <@deltanedas:kde.org>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 ShadowCommander <65ShadowCommander@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Guidebook;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization;

namespace Content.Shared.Roles;

/// <summary>
///     Describes information for a single antag.
/// </summary>
[Prototype]
[Serializable, NetSerializable]
public sealed partial class AntagPrototype : IPrototype
{
    [ViewVariables]
    [IdDataField]
    public string ID { get; private set; } = default!;

    /// <summary>
    ///     The name of this antag as displayed to players.
    /// </summary>
    [DataField("name")]
    public string Name { get; private set; } = "";

    /// <summary>
    ///     The antag's objective, shown in a tooltip in the antag preference menu or as a ghost role description.
    /// </summary>
    [DataField("objective", required: true)]
    public string Objective { get; private set; } = "";

    /// <summary>
    ///     Whether or not the antag role is one of the bad guys.
    /// </summary>
    [DataField("antagonist")]
    public bool Antagonist { get; private set; }

    /// <summary>
    ///     Whether or not the player can set the antag role in antag preferences.
    /// </summary>
    [DataField("setPreference")]
    public bool SetPreference { get; private set; }

    /// <summary>
    ///     Requirements that must be met to opt in to this antag role.
    /// </summary>
    // TODO ROLE TIMERS
    // Actually check if the requirements are met. Because apparently this is actually unused.
    [DataField, Access(typeof(SharedRoleSystem), Other = AccessPermissions.None)]
    public HashSet<JobRequirement>? Requirements;

    /// <summary>
    /// Optional list of guides associated with this antag. If the guides are opened, the first entry in this list
    /// will be used to select the currently selected guidebook.
    /// </summary>
    [DataField]
    public List<ProtoId<GuideEntryPrototype>>? Guides;
}