// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 c65llv65e <65c65llv65e@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <65deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <@deltanedas:kde.org>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Content.Shared.Roles;
using Content.Shared.SprayPainter.Prototypes;
using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;

namespace Content.Shared.SprayPainter.Components;

[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class PaintableAirlockComponent : Component
{
    /// <summary>
    /// Group of styles this airlock can be painted with, e.g. glass, standard or external.
    /// </summary>
    [DataField(required: true), AutoNetworkedField]
    public ProtoId<AirlockGroupPrototype> Group = string.Empty;

    /// <summary>
    /// Department this airlock is painted as, or none.
    /// Must be specified in prototypes for turf war to work.
    /// To better catch any mistakes, you need to explicitly state a non-styled airlock has a null department.
    /// </summary>
    [DataField(required: true), AutoNetworkedField]
    public ProtoId<DepartmentPrototype>? Department;
}