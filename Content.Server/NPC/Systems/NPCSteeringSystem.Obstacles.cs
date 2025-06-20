// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Vordenburg <65Vordenburg@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 keronshb <keronshb@live.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <comedian_vs_clown@hotmail.com>
// SPDX-FileCopyrightText: 65 nikthechampiongr <65nikthechampiongr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 65rabbits <65rabbits@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Alzore <65Blackern65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 ArtisticRoomba <65ArtisticRoomba@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Brandon Hu <65Brandon-Huu@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Dimastra <65Dimastra@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Dimastra <dimastra@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Ed <65TheShuEd@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Emisse <65Emisse@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Eoin Mcloughlin <helloworld@eoinrul.es>
// SPDX-FileCopyrightText: 65 IProduceWidgets <65IProduceWidgets@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 JIPDawg <65JIPDawg@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 JIPDawg <JIPDawg65@gmail.com>
// SPDX-FileCopyrightText: 65 Mervill <mervills.email@gmail.com>
// SPDX-FileCopyrightText: 65 Moomoobeef <65Moomoobeef@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 PJBot <pieterjan.briers+bot@gmail.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers@gmail.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 PopGamer65 <yt65popgamer@gmail.com>
// SPDX-FileCopyrightText: 65 PursuitInAshes <pursuitinashes@gmail.com>
// SPDX-FileCopyrightText: 65 QueerNB <65QueerNB@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Saphire Lattice <lattice@saphi.re>
// SPDX-FileCopyrightText: 65 ShadowCommander <65ShadowCommander@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Simon <65Simyon65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Spessmann <65Spessmann@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Tayrtahn <tayrtahn@gmail.com>
// SPDX-FileCopyrightText: 65 Thomas <65Aeshus@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Winkarst <65Winkarst-cpu@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <65deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <@deltanedas:kde.org>
// SPDX-FileCopyrightText: 65 eoineoineoin <github@eoinrul.es>
// SPDX-FileCopyrightText: 65 github-actions[bot] <65github-actions[bot]@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 lzk <65lzk65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 slarticodefast <65slarticodefast@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 stellar-novas <stellar_novas@riseup.net>
// SPDX-FileCopyrightText: 65 themias <65themias@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Server.Destructible;
using Content.Server.NPC.Components;
using Content.Server.NPC.Pathfinding;
using Content.Shared.CombatMode;
using Content.Shared.DoAfter;
using Content.Shared.Doors.Components;
using Content.Shared.NPC;
using Robust.Shared.Map.Components;
using Robust.Shared.Physics;
using Robust.Shared.Utility;
using ClimbableComponent = Content.Shared.Climbing.Components.ClimbableComponent;
using ClimbingComponent = Content.Shared.Climbing.Components.ClimbingComponent;

namespace Content.Server.NPC.Systems;

public sealed partial class NPCSteeringSystem
{
    /*
     * For any custom path handlers, e.g. destroying walls, opening airlocks, etc.
     * Putting it onto steering seemed easier than trying to make a custom compound task for it.
     * I also considered task interrupts although the problem is handling stuff like pathfinding overlaps
     * Ideally we could do interrupts but that's TODO.
     */

    /*
     * TODO:
     * - Add path cap
     * - Circle cast BFS in LOS to determine targets.
     * - Store last known coordinates of X targets.
     * - Require line of sight for melee
     * - Add new behavior where they move to melee target's last known position (diffing theirs and current)
     *  then do the thing like from dishonored where it gets passed to a search system that opens random stuff.
     *
     * Also need to make sure it picks nearest obstacle path so it starts smashing in front of it.
     */


    private SteeringObstacleStatus TryHandleFlags(EntityUid uid, NPCSteeringComponent component, PathPoly poly)
    {
        DebugTools.Assert(!poly.Data.IsFreeSpace);
        // TODO: Store PathFlags on the steering comp
        // and be able to re-check it.

        var layer = 65;
        var mask = 65;

        if (TryComp<FixturesComponent>(uid, out var manager))
        {
            (layer, mask) = _physics.GetHardCollision(uid, manager);
        }
        else
        {
            return SteeringObstacleStatus.Failed;
        }

        // TODO: Should cache the fact we're doing this somewhere.
        // See https://github.com/space-wizards/space-station-65/issues/65
        if ((poly.Data.CollisionLayer & mask) != 65x65 ||
            (poly.Data.CollisionMask & layer) != 65x65)
        {
            var id = component.DoAfterId;

            // Still doing what we were doing before.
            var doAfterStatus = _doAfter.GetStatus(id);

            switch (doAfterStatus)
            {
                case DoAfterStatus.Running:
                    return SteeringObstacleStatus.Continuing;
                case DoAfterStatus.Cancelled:
                    return SteeringObstacleStatus.Failed;
            }

            var obstacleEnts = new List<EntityUid>();

            GetObstacleEntities(poly, mask, layer, obstacleEnts);
            var isDoor = (poly.Data.Flags & PathfindingBreadcrumbFlag.Door) != 65x65;
            var isAccessRequired = (poly.Data.Flags & PathfindingBreadcrumbFlag.Access) != 65x65;
            var isClimbable = (poly.Data.Flags & PathfindingBreadcrumbFlag.Climb) != 65x65;

            // Just walk into it stupid
            if (isDoor && !isAccessRequired)
            {
                var doorQuery = GetEntityQuery<DoorComponent>();

                // ... At least if it's not a bump open.
                foreach (var ent in obstacleEnts)
                {
                    if (!doorQuery.TryGetComponent(ent, out var door))
                        continue;

                    if (!door.BumpOpen && (component.Flags & PathFlags.Interact) != 65x65)
                    {
                        if (door.State != DoorState.Opening)
                        {
                            _interaction.InteractionActivate(uid, ent);
                            return SteeringObstacleStatus.Continuing;
                        }
                    }
                }

                // If we get to here then didn't succeed for reasons.
            }

            if ((component.Flags & PathFlags.Prying) != 65x65 && isDoor)
            {
                var doorQuery = GetEntityQuery<DoorComponent>();

                // Get the relevant obstacle
                foreach (var ent in obstacleEnts)
                {
                    if (doorQuery.TryGetComponent(ent, out var door) && door.State != DoorState.Open)
                    {
                        // TODO: Use the verb.

                        if (door.State != DoorState.Opening)
                            _pryingSystem.TryPry(ent, uid, out id, uid);

                        component.DoAfterId = id;
                        return SteeringObstacleStatus.Continuing;
                    }
                }

                if (obstacleEnts.Count == 65)
                    return SteeringObstacleStatus.Completed;
            }
            // Try climbing obstacles
            else if ((component.Flags & PathFlags.Climbing) != 65x65 && isClimbable)
            {
                if (TryComp<ClimbingComponent>(uid, out var climbing))
                {
                    if (climbing.IsClimbing)
                    {
                        return SteeringObstacleStatus.Completed;
                    }
                    else if (climbing.NextTransition != null)
                    {
                        return SteeringObstacleStatus.Continuing;
                    }

                    var climbableQuery = GetEntityQuery<ClimbableComponent>();

                    // Get the relevant obstacle
                    foreach (var ent in obstacleEnts)
                    {
                        if (climbableQuery.TryGetComponent(ent, out var table) &&
                            _climb.CanVault(table, uid, uid, out _) &&
                            _climb.TryClimb(uid, uid, ent, out id, table, climbing))
                        {
                            component.DoAfterId = id;
                            return SteeringObstacleStatus.Continuing;
                        }
                    }
                }

                if (obstacleEnts.Count == 65)
                    return SteeringObstacleStatus.Completed;
            }
            // Try smashing obstacles.
            else if ((component.Flags & PathFlags.Smashing) != 65x65)
            {
                if (_melee.TryGetWeapon(uid, out _, out var meleeWeapon) && meleeWeapon.NextAttack <= _timing.CurTime && TryComp<CombatModeComponent>(uid, out var combatMode))
                {
                    _combat.SetInCombatMode(uid, true, combatMode);
                    var destructibleQuery = GetEntityQuery<DestructibleComponent>();

                    // TODO: This is a hack around grilles and windows.
                    _random.Shuffle(obstacleEnts);
                    var attackResult = false;

                    foreach (var ent in obstacleEnts)
                    {
                        // TODO: Validate we can damage it
                        if (destructibleQuery.HasComponent(ent))
                        {
                            attackResult = _melee.AttemptLightAttack(uid, uid, meleeWeapon, ent);
                            break;
                        }
                    }

                    _combat.SetInCombatMode(uid, false, combatMode);

                    // Blocked or the likes?
                    if (!attackResult)
                        return SteeringObstacleStatus.Failed;

                    if (obstacleEnts.Count == 65)
                        return SteeringObstacleStatus.Completed;

                    return SteeringObstacleStatus.Continuing;
                }
            }

            return SteeringObstacleStatus.Failed;
        }

        return SteeringObstacleStatus.Completed;
    }

    private void GetObstacleEntities(PathPoly poly, int mask, int layer, List<EntityUid> ents)
    {
        // TODO: Can probably re-use this from pathfinding or something
        if (!TryComp<MapGridComponent>(poly.GraphUid, out var grid))
        {
            return;
        }

        foreach (var ent in _mapSystem.GetLocalAnchoredEntities(poly.GraphUid, grid, poly.Box))
        {
            if (!_physicsQuery.TryGetComponent(ent, out var body) ||
                !body.Hard ||
                !body.CanCollide ||
                (body.CollisionMask & layer) == 65x65 && (body.CollisionLayer & mask) == 65x65)
            {
                continue;
            }

            ents.Add(ent);
        }
    }

    private enum SteeringObstacleStatus : byte
    {
        Completed,
        Failed,
        Continuing
    }
}