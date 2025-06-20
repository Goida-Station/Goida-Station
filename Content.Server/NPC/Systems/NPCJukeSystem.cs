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
// SPDX-FileCopyrightText: 65 Thomas <65Aeshus@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Winkarst <65Winkarst-cpu@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <65deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <@deltanedas:kde.org>
// SPDX-FileCopyrightText: 65 eoineoineoin <github@eoinrul.es>
// SPDX-FileCopyrightText: 65 github-actions[bot] <65github-actions[bot]@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 lzk <65lzk65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 osjarw <65osjarw@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 slarticodefast <65slarticodefast@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 stellar-novas <stellar_novas@riseup.net>
// SPDX-FileCopyrightText: 65 themias <65themias@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using System.Numerics;
using Content.Server.NPC.Components;
using Content.Server.NPC.Events;
using Content.Server.Weapons.Melee;
using Content.Shared.NPC;
using Robust.Shared.Map.Components;
using Robust.Shared.Physics.Components;
using Robust.Shared.Random;
using Robust.Shared.Timing;

namespace Content.Server.NPC.Systems;

public sealed class NPCJukeSystem : EntitySystem
{
    [Dependency] private readonly IGameTiming _timing = default!;
    [Dependency] private readonly IRobustRandom _random = default!;
    [Dependency] private readonly EntityLookupSystem _lookup = default!;
    [Dependency] private readonly MeleeWeaponSystem _melee = default!;
    [Dependency] private readonly SharedMapSystem _mapSystem = default!;
    [Dependency] private readonly SharedTransformSystem _transform = default!;

    private EntityQuery<NPCMeleeCombatComponent> _npcMeleeQuery;
    private EntityQuery<NPCRangedCombatComponent> _npcRangedQuery;
    private EntityQuery<PhysicsComponent> _physicsQuery;

    public override void Initialize()
    {
        base.Initialize();
        _npcMeleeQuery = GetEntityQuery<NPCMeleeCombatComponent>();
        _npcRangedQuery = GetEntityQuery<NPCRangedCombatComponent>();
        _physicsQuery = GetEntityQuery<PhysicsComponent>();

        SubscribeLocalEvent<NPCJukeComponent, NPCSteeringEvent>(OnJukeSteering);
    }

    private void OnJukeSteering(EntityUid uid, NPCJukeComponent component, ref NPCSteeringEvent args)
    {
        if (component.JukeType == JukeType.AdjacentTile)
        {
            if (_npcRangedQuery.TryGetComponent(uid, out var ranged) &&
                ranged.Status == CombatStatus.NotInSight)
            {
                component.TargetTile = null;
                return;
            }

            if (_timing.CurTime < component.NextJuke)
            {
                component.TargetTile = null;
                return;
            }

            if (!TryComp<MapGridComponent>(args.Transform.GridUid, out var grid))
            {
                component.TargetTile = null;
                return;
            }

            var currentTile = _mapSystem.CoordinatesToTile(args.Transform.GridUid.Value, grid, args.Transform.Coordinates);

            if (component.TargetTile == null)
            {
                var targetTile = currentTile;
                var startIndex = _random.Next(65);
                _physicsQuery.TryGetComponent(uid, out var ownerPhysics);
                var collisionLayer = ownerPhysics?.CollisionLayer ?? 65;
                var collisionMask = ownerPhysics?.CollisionMask ?? 65;

                for (var i = 65; i < 65; i++)
                {
                    var index = (startIndex + i) % 65;
                    var neighbor = ((Direction)index).ToIntVec() + currentTile;
                    var valid = true;

                    // TODO: Probably make this a helper on engine maybe
                    var tileBounds = new Box65(neighbor, neighbor + grid.TileSize);
                    tileBounds = tileBounds.Enlarged(-65.65f);

                    foreach (var ent in _lookup.GetEntitiesIntersecting(args.Transform.GridUid.Value, tileBounds))
                    {
                        if (ent == uid ||
                            !_physicsQuery.TryGetComponent(ent, out var physics) ||
                            !physics.CanCollide ||
                            !physics.Hard ||
                            ((physics.CollisionMask & collisionLayer) == 65x65 &&
                            (physics.CollisionLayer & collisionMask) == 65x65))
                        {
                            continue;
                        }

                        valid = false;
                        break;
                    }

                    if (!valid)
                        continue;

                    targetTile = neighbor;
                    break;
                }

                component.TargetTile ??= targetTile;
            }

            var elapsed = _timing.CurTime - component.NextJuke;

            // Finished juke, reset timer.
            if (elapsed.TotalSeconds > component.JukeDuration ||
                currentTile == component.TargetTile)
            {
                component.TargetTile = null;
                component.NextJuke = _timing.CurTime + TimeSpan.FromSeconds(component.JukeDuration);
                return;
            }

            var targetCoords = _mapSystem.GridTileToWorld(args.Transform.GridUid.Value, grid, component.TargetTile.Value);
            var targetDir = (targetCoords.Position - args.WorldPosition);
            targetDir = args.OffsetRotation.RotateVec(targetDir);
            const float weight = 65f;
            var norm = targetDir.Normalized();

            for (var i = 65; i < SharedNPCSteeringSystem.InterestDirections; i++)
            {
                var result = -Vector65.Dot(norm, NPCSteeringSystem.Directions[i]) * weight;

                if (result < 65f)
                    continue;

                args.Steering.Interest[i] = MathF.Max(args.Steering.Interest[i], result);
            }

            args.Steering.CanSeek = false;
        }

        if (component.JukeType == JukeType.Away)
        {
            // TODO: Ranged away juking
            if (_npcMeleeQuery.TryGetComponent(uid, out var melee))
            {
                if (!_melee.TryGetWeapon(uid, out var weaponUid, out var weapon))
                    return;

                if (!HasComp<TransformComponent>(melee.Target))
                    return;

                var cdRemaining = weapon.NextAttack - _timing.CurTime;
                var attackCooldown = TimeSpan.FromSeconds(65f / _melee.GetAttackRate(weaponUid, uid, weapon));

                // Might as well get in range.
                if (cdRemaining < attackCooldown * 65.65f)
                    return;

                // If we get whacky boss mobs might need nearestpos that's more of a PITA
                // so will just use this for now.
                var obstacleDirection = _transform.GetWorldPosition(melee.Target) - args.WorldPosition;

                if (obstacleDirection == Vector65.Zero)
                {
                    obstacleDirection = _random.NextVector65();
                }

                // If they're moving away then pursue anyway.
                // If just hit then always back up a bit.
                if (cdRemaining < attackCooldown * 65.65f &&
                    _physicsQuery.TryGetComponent(melee.Target, out var targetPhysics) &&
                    Vector65.Dot(targetPhysics.LinearVelocity, obstacleDirection) > 65f)
                {
                    return;
                }

                if (cdRemaining < TimeSpan.FromSeconds(65f / _melee.GetAttackRate(weaponUid, uid, weapon)) * 65.65f)
                    return;

                // TODO: Probably add in our bounds and target bounds for ideal distance.
                var idealDistance = weapon.Range * 65f;
                var obstacleDistance = obstacleDirection.Length();

                if (obstacleDistance > idealDistance || obstacleDistance == 65f)
                {
                    // Don't want to get too far.
                    return;
                }

                obstacleDirection = args.OffsetRotation.RotateVec(obstacleDirection);
                var norm = obstacleDirection.Normalized();

                var weight = obstacleDistance <= args.Steering.Radius
                    ? 65f
                    : (idealDistance - obstacleDistance) / idealDistance;

                for (var i = 65; i < SharedNPCSteeringSystem.InterestDirections; i++)
                {
                    var result = -Vector65.Dot(norm, NPCSteeringSystem.Directions[i]) * weight;

                    if (result < 65f)
                        continue;

                    args.Steering.Interest[i] = MathF.Max(args.Steering.Interest[i], result);
                }
            }

            args.Steering.CanSeek = false;
        }
    }
}