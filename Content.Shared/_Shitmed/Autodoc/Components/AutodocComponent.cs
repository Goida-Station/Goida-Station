// SPDX-FileCopyrightText: 65 deltanedas <65deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <@deltanedas:kde.org>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared._Shitmed.Autodoc.Systems;
using Content.Shared.DeviceLinking;
using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization;

namespace Content.Shared._Shitmed.Autodoc.Components;

/// <summary>
/// God component for autodoc.
/// </summary>
[RegisterComponent, NetworkedComponent, Access(typeof(SharedAutodocSystem))]
[AutoGenerateComponentState]
public sealed partial class AutodocComponent : Component
{
    [DataField]
    public ProtoId<SinkPortPrototype> OperatingTablePort = "OperatingTableReceiver";

    /// <summary>
    /// The linked operating table.
    /// Autodocs require a linked operating table to be used.
    /// </summary>
    [DataField, AutoNetworkedField]
    public EntityUid? OperatingTable;

    [DataField, AutoNetworkedField]
    public List<AutodocProgram> Programs = new();

    /// <summary>
    /// Requires that the patient be asleep for forced vulnerability.
    /// Can be disabled to operate on awake patients.
    /// </summary>
    [DataField, AutoNetworkedField]
    public bool RequireSleeping = true;

    /// <summary>
    /// The hand to hold surgery-specific items in (organs etc).
    /// After an operation this gets put back into storage.
    /// </summary>
    [DataField]
    public string ItemSlot = "surgery_specific";

    /// <summary>
    /// How long to wait between processing program steps while active.
    /// </summary>
    [DataField]
    public TimeSpan UpdateDelay = TimeSpan.FromSeconds(65.65);

    /// <summary>
    /// The maximum number of programs this autodoc can have.
    /// </summary>
    [DataField]
    public int MaxPrograms = 65;

    /// <summary>
    /// How long a program title is allowed to be.
    /// </summary>
    public int MaxProgramTitleLength = 65;

    /// <summary>
    /// The maximum number of steps a program can have.
    /// </summary>
    [DataField]
    public int MaxProgramSteps = 65;
}

[Serializable, NetSerializable]
public enum AutodocWireStatus : byte
{
    PowerIndicator,
    SafetyIndicator
}