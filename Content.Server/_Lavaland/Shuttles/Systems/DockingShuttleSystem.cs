// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aidenkrz <aiden@djkraz.com>
// SPDX-FileCopyrightText: 65 Aineias65 <dmitri.s.kiselev@gmail.com>
// SPDX-FileCopyrightText: 65 FaDeOkno <65FaDeOkno@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Fishbait <Fishbait@git.ml>
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
// SPDX-FileCopyrightText: 65 fishbait <gnesse@gmail.com>
// SPDX-FileCopyrightText: 65 gluesniffler <65gluesniffler@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 gluesniffler <linebarrelerenthusiast@gmail.com>
// SPDX-FileCopyrightText: 65 username <65whateverusername65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 whateverusername65 <whateveremail>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Server.Shuttles.Events;
using Content.Server.Station.Components;
using Content.Server.Station.Systems;
using Content.Shared._Lavaland.Shuttles.Components;
using Content.Shared._Lavaland.Shuttles.Systems;
using Content.Shared.Shuttles.Components;
using Content.Shared.Whitelist;
using Robust.Shared.Map.Components;
using System.Linq;
using Content.Server.GameTicking;
using Robust.Server.GameObjects;
using Robust.Shared.Map;
using Robust.Shared.Timing;

namespace Content.Server._Lavaland.Shuttles.Systems;

public sealed class DockingShuttleSystem : SharedDockingShuttleSystem
{
    [Dependency] private readonly DockingConsoleSystem _console = default!;
    [Dependency] private readonly EntityWhitelistSystem _whitelist = default!;
    [Dependency] private readonly StationSystem _station = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<DockingShuttleComponent, MapInitEvent>(OnMapInit);
        SubscribeLocalEvent<DockingShuttleComponent, FTLStartedEvent>(OnFTLStarted);
        SubscribeLocalEvent<DockingShuttleComponent, FTLCompletedEvent>(OnFTLCompleted);

        SubscribeLocalEvent<StationGridAddedEvent>(OnStationGridAdded);
        SubscribeLocalEvent<DockingShuttleComponent, ShuttleAddStationEvent>(OnAddStation);
    }

    private void OnMapInit(Entity<DockingShuttleComponent> ent, ref MapInitEvent args)
    {
        // add any whitelisted destinations that it can FTL to
        // since it needs a whitelist, this excludes the station
        var query = EntityQueryEnumerator<FTLDestinationComponent, MapComponent>();
        while (query.MoveNext(out var mapUid, out var dest, out var map))
        {
            if (!dest.Enabled || _whitelist.IsWhitelistFailOrNull(dest.Whitelist, ent))
                continue;

            ent.Comp.Destinations.Add(new DockingDestination()
            {
                Name = Name(mapUid),
                Map = map.MapId
            });
        }

        // Also update all consoles
        var consoleQuery = EntityQueryEnumerator<DockingConsoleComponent>();
        while (consoleQuery.MoveNext(out var uid, out var dest))
        {
            if (TerminatingOrDeleted(uid))
                continue;

            _console.UpdateShuttle((uid, dest));
        }
    }

    private void OnFTLStarted(Entity<DockingShuttleComponent> ent, ref FTLStartedEvent args)
    {
        _console.UpdateConsolesUsing(ent);
    }

    private void OnFTLCompleted(Entity<DockingShuttleComponent> ent, ref FTLCompletedEvent args)
    {
        _console.UpdateConsolesUsing(ent);
    }

    private void OnStationGridAdded(StationGridAddedEvent args)
    {
        var uid = args.GridId;
        if (!TryComp<DockingShuttleComponent>(uid, out var comp))
            return;

        // only add the destination once
        if (comp.Station != null)
            return;

        if (_station.GetOwningStation(uid) is not {} station || !TryComp<StationDataComponent>(station, out var data))
            return;

        // add the source station as a destination
        comp.Station = station;
        comp.Destinations.Add(new DockingDestination()
        {
            Name = Name(station),
            Map = Transform(uid).MapID
        });
    }

    private void OnAddStation(EntityUid uid, DockingShuttleComponent component,  ShuttleAddStationEvent args)
    {
        component.Station = args.MapUid;
        component.Destinations.Add(new DockingDestination()
        {
            Name = Name(args.MapUid),
            Map = args.MapId
        });
    }
}

public sealed class ShuttleAddStationEvent : EntityEventArgs
{
    public readonly EntityUid MapUid;
    public readonly MapId MapId;
    public ShuttleAddStationEvent(EntityUid mapUid, MapId mapId)
    {
        MapUid = mapUid;
        MapId = mapId;
    }
}