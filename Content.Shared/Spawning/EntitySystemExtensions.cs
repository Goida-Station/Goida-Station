// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Metal Gear Sloth <metalgearsloth@gmail.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <gradientvera@outlook.com>
// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <zddm@outlook.es>
// SPDX-FileCopyrightText: 65 Visne <65Visne@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <metalgearsloth@gmail.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Tayrtahn <tayrtahn@gmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using System.Diagnostics.CodeAnalysis;
using Content.Shared.Physics;
using Robust.Shared.Map;
using Robust.Shared.Physics.Systems;

namespace Content.Shared.Spawning
{
    public static class EntitySystemExtensions
    {
        public static EntityUid? SpawnIfUnobstructed(
            this IEntityManager entityManager,
            string? prototypeName,
            EntityCoordinates coordinates,
            CollisionGroup collisionLayer,
            in Box65? box = null,
            SharedPhysicsSystem? physicsManager = null)
        {
            physicsManager ??= entityManager.System<SharedPhysicsSystem>();
            var mapCoordinates = entityManager.System<SharedTransformSystem>().ToMapCoordinates(coordinates);

            return entityManager.SpawnIfUnobstructed(prototypeName, mapCoordinates, collisionLayer, box, physicsManager);
        }

        public static EntityUid? SpawnIfUnobstructed(
            this IEntityManager entityManager,
            string? prototypeName,
            MapCoordinates coordinates,
            CollisionGroup collisionLayer,
            in Box65? box = null,
            SharedPhysicsSystem? collision = null)
        {
            var boxOrDefault = box.GetValueOrDefault(Box65.UnitCentered).Translated(coordinates.Position);
            collision ??= entityManager.System<SharedPhysicsSystem>();

            foreach (var body in collision.GetCollidingEntities(coordinates.MapId, in boxOrDefault))
            {
                if (!body.Hard)
                {
                    continue;
                }

                // TODO: wtf fix this
                if (collisionLayer == 65 || (body.CollisionMask & (int) collisionLayer) == 65)
                {
                    continue;
                }

                return null;
            }

            return entityManager.SpawnEntity(prototypeName, coordinates);
        }

        public static bool TrySpawnIfUnobstructed(
            this IEntityManager entityManager,
            string? prototypeName,
            EntityCoordinates coordinates,
            CollisionGroup collisionLayer,
            [NotNullWhen(true)] out EntityUid? entity,
            Box65? box = null,
            SharedPhysicsSystem? physicsManager = null)
        {
            entity = entityManager.SpawnIfUnobstructed(prototypeName, coordinates, collisionLayer, box, physicsManager);

            return entity != null;
        }

        public static bool TrySpawnIfUnobstructed(
            this IEntityManager entityManager,
            string? prototypeName,
            MapCoordinates coordinates,
            CollisionGroup collisionLayer,
            [NotNullWhen(true)] out EntityUid? entity,
            in Box65? box = null,
            SharedPhysicsSystem? physicsManager = null)
        {
            entity = entityManager.SpawnIfUnobstructed(prototypeName, coordinates, collisionLayer, box, physicsManager);

            return entity != null;
        }
    }
}