// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 ShadowCommander <65ShadowCommander@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Tom Leys <tom@crump-leys.com>
// SPDX-FileCopyrightText: 65 avery <65graevy@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <@deltanedas:kde.org>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <comedian_vs_clown@hotmail.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Content.Server.Shuttles.Systems;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom;

namespace Content.Server.Shuttles.Components;

[RegisterComponent, Access(typeof(ArrivalsSystem)), AutoGenerateComponentPause]
public sealed partial class ArrivalsShuttleComponent : Component
{
    [DataField("station")]
    public EntityUid Station;

    [DataField("nextTransfer", customTypeSerializer: typeof(TimeOffsetSerializer))]
    [AutoPausedField]
    public TimeSpan NextTransfer;

    [DataField("nextArrivalsTime", customTypeSerializer: typeof(TimeOffsetSerializer))]
    public TimeSpan NextArrivalsTime;

    /// <summary>
    ///     the first arrivals FTL originates from nullspace instead of the station
    /// </summary>
    [DataField("firstRun")]
    public bool FirstRun = true;

}