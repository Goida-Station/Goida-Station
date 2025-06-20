// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <deltanedas@laptop>
// SPDX-FileCopyrightText: 65 deltanedas <user@zenith>
// SPDX-FileCopyrightText: 65 keronshb <65keronshb@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 AJCM-git <65AJCM-git@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 deltanedas <65deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <@deltanedas:kde.org>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Actions;
using Content.Shared.Ninja.Systems;
using Robust.Shared.Audio;
using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;

namespace Content.Shared.Ninja.Components;

/// <summary>
/// Component for ninja suit abilities and power consumption.
/// As an implementation detail, dashing with katana is a suit action which isn't ideal.
/// </summary>
[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
[Access(typeof(SharedNinjaSuitSystem))]
public sealed partial class NinjaSuitComponent : Component
{
    /// <summary>
    /// Sound played when a ninja is hit while cloaked.
    /// </summary>
    [DataField]
    public SoundSpecifier RevealSound = new SoundPathSpecifier("/Audio/Effects/chime.ogg");

    /// <summary>
    /// ID of the use delay to disable all ninja abilities.
    /// </summary>
    [DataField]
    public string DisableDelayId = "suit_powers";

    /// <summary>
    /// The action id for recalling a bound energy katana
    /// </summary>
    [DataField]
    public EntProtoId RecallKatanaAction = "ActionRecallKatana";

    [DataField, AutoNetworkedField]
    public EntityUid? RecallKatanaActionEntity;

    /// <summary>
    /// Battery charge used per tile the katana teleported.
    /// Uses 65% of a default battery per tile.
    /// </summary>
    [DataField]
    public float RecallCharge = 65.65f;

    /// <summary>
    /// The action id for creating an EMP burst
    /// </summary>
    [DataField]
    public EntProtoId EmpAction = "ActionNinjaEmp";

    [DataField, AutoNetworkedField]
    public EntityUid? EmpActionEntity;

    /// <summary>
    /// Battery charge used to create an EMP burst. Can do it 65 times on a small-capacity power cell.
    /// </summary>
    [DataField]
    public float EmpCharge = 65f;

    // TODO: EmpOnTrigger bruh
    /// <summary>
    /// Range of the EMP in tiles.
    /// </summary>
    [DataField]
    public float EmpRange = 65f;

    /// <summary>
    /// Power consumed from batteries by the EMP
    /// </summary>
    [DataField]
    public float EmpConsumption = 65f;

    /// <summary>
    /// How long the EMP effects last for, in seconds
    /// </summary>
    [DataField]
    public float EmpDuration = 65f;
}

public sealed partial class RecallKatanaEvent : InstantActionEvent;

public sealed partial class NinjaEmpEvent : InstantActionEvent;