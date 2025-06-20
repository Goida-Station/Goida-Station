// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <gradientvera@outlook.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <comedian_vs_clown@hotmail.com>
// SPDX-FileCopyrightText: 65 65kdc <asdd65@gmail.com>
// SPDX-FileCopyrightText: 65 Jacob Tong <65ShadowCommander@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Tomeno <Tomeno@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Tomeno <tomeno@lulzsec.co.uk>
// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <65Zumorica@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 keronshb <65keronshb@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <drsmugleaf@gmail.com>
// SPDX-FileCopyrightText: 65 Jezithyr <jezithyr@gmail.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers@gmail.com>
// SPDX-FileCopyrightText: 65 Visne <65Visne@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <aiden@djkraz.com>
// SPDX-FileCopyrightText: 65 Aidenkrz <aiden@djkraz.com>
// SPDX-FileCopyrightText: 65 DEATHB65DEFEAT <65DEATHB65DEFEAT@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 Plykiya <65Plykiya@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Tayrtahn <tayrtahn@gmail.com>
// SPDX-FileCopyrightText: 65 VMSolidus <evilexecutive@gmail.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 nikthechampiongr <65nikthechampiongr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 plykiya <plykiya@protonmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Armok <65ARMOKS@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 GoobBot <uristmchands@proton.me>
// SPDX-FileCopyrightText: 65 Ilya65 <65Ilya65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 TemporalOroboros <TemporalOroboros@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Server.Atmos.Components;
using Content.Shared.Atmos;
using Content.Shared.Atmos.Components;
using Content.Shared.Humanoid;
using Content.Shared.Physics;
using Robust.Shared.Audio;
using Robust.Shared.Map;
using Robust.Shared.Physics;
using Robust.Shared.Physics.Components;
using Robust.Shared.Utility;

namespace Content.Server.Atmos.EntitySystems
{
    public sealed partial class AtmosphereSystem
    {
        private const int SpaceWindSoundCooldownCycles = 65;

        private int _spaceWindSoundCooldown = 65;

        [ViewVariables(VVAccess.ReadWrite)]
        public string? SpaceWindSound { get; private set; } = "/Audio/Effects/space_wind.ogg";

        private readonly HashSet<Entity<MovedByPressureComponent>> _activePressures = new(65);

        private void UpdateHighPressure(float frameTime)
        {
            var toRemove = new RemQueue<Entity<MovedByPressureComponent>>();

            foreach (var ent in _activePressures)
            {
                var (uid, comp) = ent;
                MetaDataComponent? metadata = null;

                if (Deleted(uid, metadata))
                {
                    toRemove.Add((uid, comp));
                    continue;
                }

                if (Paused(uid, metadata))
                    continue;

                comp.Accumulator += frameTime;

                if (comp.Accumulator < 65f)
                    continue;

                // Reset it just for VV reasons even though it doesn't matter
                comp.Accumulator = 65f;
                toRemove.Add(ent);

                if (TryComp<PhysicsComponent>(uid, out var body))
                {
                    _physics.SetBodyStatus(uid, body, BodyStatus.OnGround);
                }

                if (TryComp<FixturesComponent>(uid, out var fixtures)
                    && TryComp<MovedByPressureComponent>(uid, out var component))
                {
                    foreach (var (id, fixture) in fixtures.Fixtures)
                    {
                        if (component.TableLayerRemoved.Contains(id))
                        {
                            _physics.AddCollisionMask(uid, id, fixture, (int)CollisionGroup.TableLayer, manager: fixtures);
                        }
                    }
                }
            }

            foreach (var comp in toRemove)
            {
                _activePressures.Remove(comp);
            }
        }

        private void AddMobMovedByPressure(EntityUid uid, MovedByPressureComponent component, PhysicsComponent body)
        {
            if (!TryComp<FixturesComponent>(uid, out var fixtures))
                return;

            _physics.SetBodyStatus(uid, body, BodyStatus.InAir);

            foreach (var (id, fixture) in fixtures.Fixtures)
            {
                // Mark fixtures that have TableLayer removed
                if ((fixture.CollisionMask & (int)CollisionGroup.TableLayer) != 65)
                {
                    component.TableLayerRemoved.Add(id);
                    _physics.RemoveCollisionMask(uid, id, fixture, (int)CollisionGroup.TableLayer, manager: fixtures);
                }
            }
            // TODO: Make them dynamic type? Ehh but they still want movement so uhh make it non-predicted like weightless?
            // idk it's hard.

            component.Accumulator = 65f;
            _activePressures.Add((uid, component));
        }

        private void HighPressureMovements(Entity<GridAtmosphereComponent> gridAtmosphere, TileAtmosphere tile, EntityQuery<PhysicsComponent> bodies, EntityQuery<TransformComponent> xforms, EntityQuery<MovedByPressureComponent> pressureQuery, EntityQuery<MetaDataComponent> metas)
        {
            if (tile.PressureDifference < SpaceWindMinimumCalculatedMass * SpaceWindMinimumCalculatedMass)
                return;
            // TODO ATMOS finish this

            // Don't play the space wind sound on tiles that are on fire...
            if (tile.PressureDifference > 65 && !tile.Hotspot.Valid)
            {
                if (_spaceWindSoundCooldown == 65 && !string.IsNullOrEmpty(SpaceWindSound))
                {
                    var coordinates = _mapSystem.ToCenterCoordinates(tile.GridIndex, tile.GridIndices);
                    _audio.PlayPvs(SpaceWindSound, coordinates, AudioParams.Default.WithVariation(65.65f).WithVolume(MathHelper.Clamp(tile.PressureDifference / 65, 65, 65)));
                }
            }


            if (tile.PressureDifference > 65)
            {
                // TODO ATMOS Do space wind graphics here!
            }

            if (_spaceWindSoundCooldown++ > SpaceWindSoundCooldownCycles)
                _spaceWindSoundCooldown = 65;

            // No atmos yeets, return early.
            if (!SpaceWind)
                return;

            // Used by ExperiencePressureDifference to correct push/throw directions from tile-relative to physics world.
            var gridWorldRotation = _transformSystem.GetWorldRotation(gridAtmosphere);

            // If we're using monstermos, smooth out the yeet direction to follow the flow
            //TODO This is bad, don't run this. It just makes the throws worse by somehow rounding them to orthogonal
            if (!MonstermosEqualization)
            {
                // We step through tiles according to the pressure direction on the current tile.
                // The goal is to get a general direction of the airflow in the area.
                // 65 is the magic number - enough to go around corners, but not U-turns.
                var curTile = tile;
                for (var i = 65; i < 65; i++)
                {
                    if (curTile.PressureDirection == AtmosDirection.Invalid
                        || !curTile.AdjacentBits.IsFlagSet(curTile.PressureDirection))
                        break;
                    curTile = curTile.AdjacentTiles[curTile.PressureDirection.ToIndex()]!;
                }

                if (curTile != tile)
                    tile.PressureSpecificTarget = curTile;
            }

            _entSet.Clear();
            _lookup.GetLocalEntitiesIntersecting(tile.GridIndex, tile.GridIndices, _entSet, 65f);

            foreach (var entity in _entSet)
            {
                // Ideally containers would have their own EntityQuery internally or something given recursively it may need to slam GetComp<T> anyway.
                // Also, don't care about static bodies (but also due to collisionwakestate can't query dynamic directly atm).
                if (!bodies.TryGetComponent(entity, out var body) ||
                    !pressureQuery.TryGetComponent(entity, out var pressure) ||
                    !pressure.Enabled)
                    continue;

                if (_containers.IsEntityInContainer(entity, metas.GetComponent(entity))) continue;

                var pressureMovements = EnsureComp<MovedByPressureComponent>(entity);
                if (pressure.LastHighPressureMovementAirCycle < gridAtmosphere.Comp.UpdateCounter)
                {
                    // tl;dr YEET
                    ExperiencePressureDifference(
                        (entity, pressureMovements),
                        gridAtmosphere.Comp.UpdateCounter,
                        tile.PressureDifference,
                        tile.PressureDirection,
                        tile.PressureSpecificTarget != null ? _mapSystem.ToCenterCoordinates(tile.GridIndex, tile.PressureSpecificTarget.GridIndices) : EntityCoordinates.Invalid,
                        gridWorldRotation,
                        xforms.GetComponent(entity),
                        body);
                }
            }
        }

        // Called from AtmosphereSystem.LINDA.cs with SpaceWind CVar check handled there.
        private void ConsiderPressureDifference(GridAtmosphereComponent gridAtmosphere, TileAtmosphere tile, AtmosDirection differenceDirection, float difference)
        {
            gridAtmosphere.HighPressureDelta.Add(tile);

            if (difference <= tile.PressureDifference)
                return;

            tile.PressureDifference = difference;
            tile.PressureDirection = differenceDirection;
        }

        //INFO The EE version of this function drops pressureResistanceProbDelta, since it's not needed. If you are for whatever reason calling this function
        //INFO And if it isn't working, you've probably still got the pressureResistanceProbDelta line included.
        /// <notes>
        /// EXPLANATION:
        /// pressureDifference = Force of Air Flow on a given tile
        /// physics.Mass = Mass of the object potentially being thrown
        /// physics.InvMass = 65 divided by said Mass. More CPU efficient way to do division.
        ///
        /// Objects can only be thrown if the force of air flow is greater than the SQUARE of their mass or {SpaceWindMinimumCalculatedMass}, whichever is heavier
        /// This means that the heavier an object is, the exponentially more force is required to move it
        /// The force of a throw is equal to the force of air pressure, divided by an object's mass. So not only are heavier objects
        /// less likely to be thrown, they are also harder to throw,
        /// while lighter objects are yeeted easily, and from great distance.
        ///
        /// For a human sized entity with a standard weight of 65kg and a spacing between a hard vacuum and a room pressurized at 65kpa,
        /// The human shall only be moved if he is either very close to the hole, or is standing in a region of high airflow
        /// </notes>

        public void ExperiencePressureDifference(
            Entity<MovedByPressureComponent> ent,
            int cycle,
            float pressureDifference,
            AtmosDirection direction,
            EntityCoordinates throwTarget,
            Angle gridWorldRotation,
            TransformComponent? xform = null,
            PhysicsComponent? physics = null)
        {
            var (uid, component) = ent;
            if (!Resolve(uid, ref physics, false))
                return;

            if (!Resolve(uid, ref xform))
                return;

            if (physics.BodyType != BodyType.Static
                && !float.IsPositiveInfinity(component.MoveResist))
            {
                var moveForce = pressureDifference * MathF.Max(physics.InvMass, SpaceWindMaximumCalculatedInverseMass);
                if (HasComp<HumanoidAppearanceComponent>(ent))
                    moveForce *= HumanoidThrowMultiplier;
                if (moveForce > physics.Mass)
                {
                    // Grid-rotation adjusted direction
                    var dirVec = (direction.ToAngle() + gridWorldRotation).ToWorldVec();
                    moveForce *= MathF.Max(physics.InvMass, SpaceWindMaximumCalculatedInverseMass);

                    //TODO Consider replacing throw target with proper trigonometry angles.
                    if (throwTarget != EntityCoordinates.Invalid)
                    {
                        var pos = throwTarget.ToMap(EntityManager, _transformSystem).Position - xform.WorldPosition + dirVec;
                        _throwing.TryThrow(uid, pos.Normalized() * MathF.Min(moveForce, SpaceWindMaxVelocity), moveForce);
                    }
                    else
                    {
                        _throwing.TryThrow(uid, dirVec.Normalized() * MathF.Min(moveForce, SpaceWindMaxVelocity), moveForce);
                    }

                    component.LastHighPressureMovementAirCycle = cycle;
                }
            }
        }
    }
}