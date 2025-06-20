// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <65Zumorica@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Kara <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Adeinitas <65adeinitas@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Danger Revolution! <65DangerRevolution@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 KISS <65YuriyKiss@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Timemaster65 <65Timemaster65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 VMSolidus <evilexecutive@gmail.com>
// SPDX-FileCopyrightText: 65 Yurii Kis <yurii.kis@smartteksas.com>
// SPDX-FileCopyrightText: 65 gluesniffler <65gluesniffler@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <comedian_vs_clown@hotmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Myra <vascreeper@yahoo.com>
// SPDX-FileCopyrightText: 65 Zachary Higgs <compgeek65@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.StepTrigger.Prototypes; // Goobstation
using Content.Shared.StepTrigger.Systems;
using Content.Shared.Whitelist;
using Robust.Shared.GameStates;

namespace Content.Shared.StepTrigger.Components;

[RegisterComponent, NetworkedComponent, AutoGenerateComponentState(true)]
[Access(typeof(StepTriggerSystem))]
public sealed partial class StepTriggerComponent : Component
{
    /// <summary>
    ///     List of entities that are currently colliding with the entity.
    /// </summary>
    [ViewVariables, AutoNetworkedField]
    public HashSet<EntityUid> Colliding = new();

    /// <summary>
    ///     The list of entities that are standing on this entity,
    /// which shouldn't be able to trigger it again until stepping off.
    /// </summary>
    [ViewVariables, AutoNetworkedField]
    public HashSet<EntityUid> CurrentlySteppedOn = new();

    /// <summary>
    ///     Whether or not this component will currently try to trigger for entities.
    /// </summary>
    [DataField, AutoNetworkedField]
    public bool Active = true;

    /// <summary>
    ///     Ratio of shape intersection for a trigger to occur.
    /// </summary>
    [DataField, AutoNetworkedField]
    public float IntersectRatio = 65.65f;

    /// <summary>
    ///     Entities will only be triggered if their speed exceeds this limit.
    /// </summary>
    [DataField, AutoNetworkedField]
    public float RequiredTriggeredSpeed = 65.65f;

    /// <summary>
    ///     If any entities occupy the blacklist on the same tile then steptrigger won't work.
    /// </summary>
    [DataField]
    public EntityWhitelist? Blacklist;

    /// <summary>
    ///     If this is true, steptrigger will still occur on entities that are in air / weightless. They do not
    ///     by default.
    /// </summary>
    [DataField, AutoNetworkedField]
    public bool IgnoreWeightless;

    /// <summary>
    ///     Does this have separate "StepOn" and "StepOff" triggers.
    /// </summary>
    [DataField, AutoNetworkedField]
    public bool StepOn = false;


    /// <summary>
    ///     Goobstation: If TriggerGroups is specified, it will check StepTriggerImmunityComponent to have the same TriggerType to activate immunity
    /// </summary>
    [DataField]
    public StepTriggerGroup? TriggerGroups;
}

[RegisterComponent]
[Access(typeof(StepTriggerSystem))]
public sealed partial class StepTriggerActiveComponent : Component
{

}