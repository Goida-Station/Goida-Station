// SPDX-FileCopyrightText: 65 Adeinitas <65adeinitas@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Danger Revolution! <65DangerRevolution@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Timemaster65 <65Timemaster65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 VMSolidus <evilexecutive@gmail.com>
// SPDX-FileCopyrightText: 65 gluesniffler <65gluesniffler@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.StepTrigger.Components;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization;

namespace Content.Shared.StepTrigger.Prototypes;

/// <summary>
///     Goobstation: A group of <see cref="StepTriggerTypePrototype">
///     Used to determine StepTriggerTypes like Tags.
///     Used for better work with Immunity.
///     StepTriggerTypes in StepTriggerTypes.yml
/// </summary>
/// <code>
/// stepTriggerGroups:
///   types:
///   - Lava
///   - Landmine
///   - Shard
///   - Chasm
///   - Mousetrap
///   - SlipTile
///   - SlipEntity
/// </code>
[DataDefinition]
[Serializable, NetSerializable]
public sealed partial class StepTriggerGroup
{
    [DataField]
    public List<ProtoId<StepTriggerTypePrototype>>? Types = null;

    /// <summary>
    ///     Checks if types of this StepTriggerGroup is similar to types of AnotherGroup
    /// </summary>
    public bool IsValid(StepTriggerGroup? anotherGroup)
    {
        if (Types is null)
            return false;

        foreach (var type in Types)
        {
            if (anotherGroup != null
                && anotherGroup.Types != null
                && anotherGroup.Types.Contains(type))
                return true;
        }
        return false;
    }

    /// <summary>
    ///     Checks validation (if types of this StepTriggerGroup are similar to types of
    ///     another StepTriggerComponent.
    /// </summary>
    public bool IsValid(StepTriggerComponent component)
    {
        if (component.TriggerGroups is null)
            return false;

        return IsValid(component.TriggerGroups);
    }

    /// <summary>
    ///     Checks validation (if types of this StepTriggerGroup are similar to types of
    ///     another StepTriggerImmuneComponent.
    /// </summary>
    public bool IsValid(StepTriggerImmuneComponent component)
    {
        if (component.Whitelist is null)
            return false;

        return IsValid(component.Whitelist);
    }
}