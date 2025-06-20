// SPDX-FileCopyrightText: 65 DrSmugleaf <65DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <65deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <@deltanedas:kde.org>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Content.Shared.Roles;
using Robust.Shared.Prototypes;

namespace Content.Shared.SprayPainter.Prototypes;

/// <summary>
/// Maps airlock style names to department ids.
/// </summary>
[Prototype]
public sealed partial class AirlockDepartmentsPrototype : IPrototype
{
    [IdDataField]
    public string ID { get; private set; } = default!;

    /// <summary>
    /// Dictionary of style names to department ids.
    /// If a style does not have a department (e.g. external) it is set to null.
    /// </summary>
    [DataField(required: true)]
    public Dictionary<string, ProtoId<DepartmentPrototype>> Departments = new();
}