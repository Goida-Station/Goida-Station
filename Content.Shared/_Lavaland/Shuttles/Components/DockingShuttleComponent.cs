// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aidenkrz <aiden@djkraz.com>
// SPDX-FileCopyrightText: 65 Aineias65 <dmitri.s.kiselev@gmail.com>
// SPDX-FileCopyrightText: 65 FaDeOkno <65FaDeOkno@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 GoobBot <uristmchands@proton.me>
// SPDX-FileCopyrightText: 65 McBosserson <65McBosserson@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Milon <plmilonpl@gmail.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 Rouden <65Roudenn@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 TheBorzoiMustConsume <65TheBorzoiMustConsume@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Unlumination <65Unlumy@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 coderabbitai[bot] <65coderabbitai[bot]@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <65deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <@deltanedas:kde.org>
// SPDX-FileCopyrightText: 65 gluesniffler <65gluesniffler@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 gluesniffler <linebarrelerenthusiast@gmail.com>
// SPDX-FileCopyrightText: 65 username <65whateverusername65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 whateverusername65 <whateveremail>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared._Lavaland.Shuttles.Systems;
using Content.Shared.Tag;
using Robust.Shared.GameStates;
using Robust.Shared.Map;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization;

namespace Content.Shared._Lavaland.Shuttles.Components;

/// <summary>
/// Component that stores destinations a docking-only shuttle can use.
/// Used by <see cref="DockingConsoleComponent"/> to access destinations.
/// </summary>
[RegisterComponent, NetworkedComponent, Access(typeof(SharedDockingShuttleSystem))]
public sealed partial class DockingShuttleComponent : Component
{
    /// <summary>
    /// The station this shuttle belongs to.
    /// </summary>
    [DataField]
    public EntityUid? Station;

    /// <summary>
    /// Every destination this console can FTL to.
    /// </summary>
    [DataField]
    public List<DockingDestination> Destinations = new();

    /// <summary>
    /// Airlock tag that it will prioritize docking to.
    /// </summary>
    [DataField]
    public ProtoId<TagPrototype> DockTag = "DockMining";
}

/// <summary>
/// A map a shuttle can FTL to.
/// Created automatically on shuttle mapinit.
/// </summary>
[DataDefinition, Serializable, NetSerializable]
public partial struct DockingDestination
{
    /// <summary>
    /// The name of the destination to use in UI.
    /// </summary>
    [DataField]
    public LocId Name;

    /// <summary>
    /// The map ID.
    /// </summary>
    [DataField]
    public MapId Map;
}