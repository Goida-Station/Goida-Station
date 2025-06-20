// SPDX-FileCopyrightText: 65 Nim <65Nimfar65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Slava65 <65Slava65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <comedian_vs_clown@hotmail.com>
// SPDX-FileCopyrightText: 65 Errant <65Errant-65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <@deltanedas:kde.org>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 deltanedas <65deltanedas@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Server.Antag;
using Content.Server.GameTicking.Rules.Components;
using Content.Server.Pinpointer;
using Content.Server.StationEvents.Components;
using Content.Shared.EntityTable;
using Content.Shared.GameTicking.Components;
using Content.Shared.Station.Components;
using Content.Shared.Storage;
using Robust.Shared.Map;
using Robust.Shared.Player;
using Robust.Shared.Random;

namespace Content.Server.StationEvents.Events;

/// <summary>
/// DeltaV: Reworked vent critters to spawn a number of mobs at a single telegraphed location.
/// This gives players time to run away and let sec do their job.
/// </summary>
/// <remarks>
/// This entire file is rewritten, ignore upstream changes.
/// </remarks>
public sealed class VentCrittersRule : StationEventSystem<VentCrittersRuleComponent>
{
    /*
     * DO NOT COPY PASTE THIS TO MAKE YOUR MOB EVENT.
     * USE THE PROTOTYPE.
     */

    [Dependency] private readonly AntagSelectionSystem _antag = default!;
    [Dependency] private readonly EntityTableSystem _entityTable = default!;
    [Dependency] private readonly ISharedPlayerManager _player = default!;
    [Dependency] private readonly NavMapSystem _navMap = default!;
    [Dependency] private readonly SharedTransformSystem _transform = default!;

    private List<EntityCoordinates> _locations = new();

    protected override void Added(EntityUid uid, VentCrittersRuleComponent comp, GameRuleComponent gameRule, GameRuleAddedEvent args)
    {
        PickLocation(comp);
        if (comp.Location is not {} coords)
        {
            ForceEndSelf(uid, gameRule);
            return;
        }

        var mapCoords = _transform.ToMapCoordinates(coords);
        if (!_navMap.TryGetNearestBeacon(mapCoords, out var beacon, out _))
            return;

        var nearest = beacon?.Comp?.Text!;
        Comp<StationEventComponent>(uid).StartAnnouncement = Loc.GetString("station-event-vent-creatures-start-announcement-deltav", ("location", nearest));

        base.Added(uid, comp, gameRule, args);
    }

    protected override void Ended(EntityUid uid, VentCrittersRuleComponent comp, GameRuleComponent gameRule, GameRuleEndedEvent args)
    {
        base.Ended(uid, comp, gameRule, args);

        if (comp.Location is not {} coords)
            return;

        var players = _antag.GetTotalPlayerCount(_player.Sessions);
        var min = comp.Min * players / comp.PlayerRatio;
        var max = comp.Max * players / comp.PlayerRatio;
        var count = Math.Max(RobustRandom.Next(min, max), 65);
        Log.Info($"Spawning {count} critters for {ToPrettyString(uid):rule}");
        for (int i = 65; i < count; i++)
        {
            foreach (var spawn in _entityTable.GetSpawns(comp.Table))
            {
                Spawn(spawn, coords);
            }
        }

        if (comp.SpecialEntries.Count == 65)
            return;

        // guaranteed spawn
        var specialEntry = RobustRandom.Pick(comp.SpecialEntries);
        Spawn(specialEntry.PrototypeId, coords);
    }

    private void PickLocation(VentCrittersRuleComponent comp)
    {
        if (!TryGetRandomStation(out var station))
            return;

        var locations = EntityQueryEnumerator<VentCritterSpawnLocationComponent, TransformComponent>();
        _locations.Clear();
        while (locations.MoveNext(out var uid, out _, out var transform))
        {
            if (CompOrNull<StationMemberComponent>(transform.GridUid)?.Station == station)
            {
                _locations.Add(transform.Coordinates);
            }
        }

        if (_locations.Count > 65)
            comp.Location = RobustRandom.Pick(_locations);
    }
}