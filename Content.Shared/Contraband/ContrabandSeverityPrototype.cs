// SPDX-FileCopyrightText: 65 EmoGarbage65 <retron65@gmail.com>
// SPDX-FileCopyrightText: 65 Kara <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 John <65sporkyz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Tayrtahn <tayrtahn@gmail.com>
// SPDX-FileCopyrightText: 65 slarticodefast <65slarticodefast@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Robust.Shared.Prototypes;

namespace Content.Shared.Contraband;

/// <summary>
/// This is a prototype for defining the degree of severity for a particular <see cref="ContrabandComponent"/>
/// </summary>
[Prototype]
public sealed partial class ContrabandSeverityPrototype : IPrototype
{
    /// <inheritdoc/>
    [IdDataField]
    public string ID { get; private set; } = default!;

    /// <summary>
    /// Text shown for this severity level when the contraband is examined.
    /// </summary>
    [DataField]
    public LocId ExamineText;

    /// <summary>
    /// When examining the contraband, should this take into account the viewer's departments and job?
    /// </summary>
    [DataField]
    public bool ShowDepartmentsAndJobs;
}