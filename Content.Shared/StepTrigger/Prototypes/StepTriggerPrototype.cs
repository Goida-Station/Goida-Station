// SPDX-FileCopyrightText: 65 Adeinitas <65adeinitas@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Danger Revolution! <65DangerRevolution@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Timemaster65 <65Timemaster65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 VMSolidus <evilexecutive@gmail.com>
// SPDX-FileCopyrightText: 65 gluesniffler <65gluesniffler@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Robust.Shared.Prototypes;

namespace Content.Shared.StepTrigger.Prototypes;

/// <summary>
///     Goobstation Change: Prototype representing a StepTriggerType in YAML.
///     Meant to only have an ID property, as that is the only thing that
///     gets saved in StepTriggerGroup.
/// </summary>
[Prototype]
public sealed partial class StepTriggerTypePrototype : IPrototype
{
    [ViewVariables, IdDataField]
    public string ID { get; private set; } = default!;
}