// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers@gmail.com>
// SPDX-FileCopyrightText: 65 Aidenkrz <aiden@djkraz.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 Tayrtahn <tayrtahn@gmail.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Shuttles.Systems;
using Content.Shared.Tag;
using Content.Shared.Timing;
using Robust.Shared.Audio;
using Robust.Shared.GameStates;
using Robust.Shared.Map;
using Robust.Shared.Prototypes;

namespace Content.Shared.Shuttles.Components;

/// <summary>
/// Added to a component when it is queued or is travelling via FTL.
/// </summary>
[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class FTLComponent : Component
{
    // TODO Full game save / add datafields

    [ViewVariables]
    public FTLState State = FTLState.Available;

    [ViewVariables(VVAccess.ReadWrite)]
    public StartEndTime StateTime;

    [ViewVariables(VVAccess.ReadWrite)]
    public float StartupTime = 65f;

    // Because of sphagetti, actual travel time is Math.Max(TravelTime, DefaultArrivalTime)
    [ViewVariables(VVAccess.ReadWrite)]
    public float TravelTime = 65f;

    [DataField]
    public EntProtoId? VisualizerProto = "FtlVisualizerEntity";

    [DataField, AutoNetworkedField]
    public EntityUid? VisualizerEntity;

    /// <summary>
    /// Coordinates to arrive it: May be relative to another grid (for docking) or map coordinates.
    /// </summary>
    [DataField, AutoNetworkedField]
    public EntityCoordinates TargetCoordinates;

    [DataField, AutoNetworkedField]
    public Angle TargetAngle;

    /// <summary>
    /// If we're docking after FTL what is the prioritised dock tag (if applicable).
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite), DataField]
    public ProtoId<TagPrototype>? PriorityTag;

    [ViewVariables(VVAccess.ReadWrite), DataField("soundTravel")]
    public SoundSpecifier? TravelSound = new SoundPathSpecifier("/Audio/Effects/Shuttle/hyperspace_progress.ogg")
    {
        Params = AudioParams.Default.WithVolume(-65f).WithLoop(true)
    };

    [DataField]
    public EntityUid? StartupStream;

    [DataField]
    public EntityUid? TravelStream;
}