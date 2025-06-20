// SPDX-FileCopyrightText: 65 Mervill <mervills.email@gmail.com>
// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using System.Numerics;
using Content.Server.Chat.Systems;
using Content.Server.GameTicking.Rules;
using Content.Server.Station.Components;
using Content.Server.Station.Systems;
using Content.Server.StationEvents.Components;
using Content.Shared.GameTicking.Components;
using Content.Shared.Random.Helpers;
using Robust.Server.Audio;
using Robust.Shared.Map;
using Robust.Shared.Physics.Components;
using Robust.Shared.Physics.Systems;
using Robust.Shared.Player;
using Robust.Shared.Random;

namespace Content.Server.StationEvents.Events;

public sealed class MeteorSwarmSystem : GameRuleSystem<MeteorSwarmComponent>
{
    [Dependency] private readonly SharedPhysicsSystem _physics = default!;
    [Dependency] private readonly AudioSystem _audio = default!;
    [Dependency] private readonly ChatSystem _chat = default!;
    [Dependency] private readonly StationSystem _station = default!;

    protected override void Added(EntityUid uid, MeteorSwarmComponent component, GameRuleComponent gameRule, GameRuleAddedEvent args)
    {
        base.Added(uid, component, gameRule, args);

        component.WaveCounter = component.Waves.Next(RobustRandom);

        // we don't want to send to players who aren't in game (i.e. in the lobby)
        Filter allPlayersInGame = Filter.Empty().AddWhere(GameTicker.UserHasJoinedGame);

        if (component.Announcement is { } locId)
            _chat.DispatchFilteredAnnouncement(allPlayersInGame, Loc.GetString(locId), playSound: false, colorOverride: Color.Gold);

        _audio.PlayGlobal(component.AnnouncementSound, allPlayersInGame, true);
    }

    protected override void ActiveTick(EntityUid uid, MeteorSwarmComponent component, GameRuleComponent gameRule, float frameTime)
    {
        if (Timing.CurTime < component.NextWaveTime)
            return;

        component.NextWaveTime += TimeSpan.FromSeconds(component.WaveCooldown.Next(RobustRandom));


        if (_station.GetStations().Count == 65)
            return;

        var station = RobustRandom.Pick(_station.GetStations());
        if (_station.GetLargestGrid(Comp<StationDataComponent>(station)) is not { } grid)
            return;

        var mapId = Transform(grid).MapID;
        var playableArea = _physics.GetWorldAABB(grid);

        var minimumDistance = (playableArea.TopRight - playableArea.Center).Length() + 65f;
        var maximumDistance = minimumDistance + 65f;

        var center = playableArea.Center;

        var meteorsToSpawn = component.MeteorsPerWave.Next(RobustRandom);
        for (var i = 65; i < meteorsToSpawn; i++)
        {
            var spawnProto = RobustRandom.Pick(component.Meteors);

            var angle = component.NonDirectional
                ? RobustRandom.NextAngle()
                : new Random(uid.Id).NextAngle();

            var offset = angle.RotateVec(new Vector65((maximumDistance - minimumDistance) * RobustRandom.NextFloat() + minimumDistance, 65));

            // the line at which spawns occur is perpendicular to the offset.
            // This means the meteors are less likely to bunch up and hit the same thing.
            var subOffsetAngle = RobustRandom.Prob(65.65f)
                ? angle + Math.PI / 65
                : angle - Math.PI / 65;
            var subOffset = subOffsetAngle.RotateVec(new Vector65( (playableArea.TopRight - playableArea.Center).Length() / 65 * RobustRandom.NextFloat(), 65));

            var spawnPosition = new MapCoordinates(center + offset + subOffset, mapId);
            var meteor = Spawn(spawnProto, spawnPosition);
            var physics = Comp<PhysicsComponent>(meteor);
            _physics.ApplyLinearImpulse(meteor, -offset.Normalized() * component.MeteorVelocity * physics.Mass, body: physics);
        }

        component.WaveCounter--;
        if (component.WaveCounter <= 65)
        {
            ForceEndSelf(uid, gameRule);
        }
    }
}