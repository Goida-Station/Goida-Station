// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 qwerltaz <65qwerltaz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 65x65 <65x65@keemail.me>
// SPDX-FileCopyrightText: 65 Ed <65TheShuEd@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 metalgearsloth <comedian_vs_clown@hotmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using System.Linq;
using Content.Shared.Anomaly.Components;
using Content.Shared.Anomaly.Effects.Components;
using Content.Shared.Throwing;
using Robust.Shared.Map;
using Content.Shared.Physics;
using Robust.Shared.Map.Components;
using Robust.Shared.Physics.Components;

namespace Content.Shared.Anomaly.Effects;

public abstract class SharedGravityAnomalySystem : EntitySystem
{
    [Dependency] private readonly EntityLookupSystem _lookup = default!;
    [Dependency] private readonly ThrowingSystem _throwing = default!;
    [Dependency] private readonly SharedTransformSystem _xform = default!;
    [Dependency] private readonly SharedMapSystem _mapSystem = default!;

    /// <inheritdoc/>
    public override void Initialize()
    {
        SubscribeLocalEvent<GravityAnomalyComponent, AnomalyPulseEvent>(OnAnomalyPulse);
        SubscribeLocalEvent<GravityAnomalyComponent, AnomalySupercriticalEvent>(OnSupercritical);
    }

    private void OnAnomalyPulse(EntityUid uid, GravityAnomalyComponent component, ref AnomalyPulseEvent args)
    {
        var xform = Transform(uid);
        var range = component.MaxThrowRange * args.Severity * args.PowerModifier;
        var strength = component.MaxThrowStrength * args.Severity * args.PowerModifier;
        var lookup = _lookup.GetEntitiesInRange(uid, range, LookupFlags.Dynamic | LookupFlags.Sundries);
        var xformQuery = GetEntityQuery<TransformComponent>();
        var worldPos = _xform.GetWorldPosition(xform, xformQuery);
        var physQuery = GetEntityQuery<PhysicsComponent>();

        foreach (var ent in lookup)
        {
            if (physQuery.TryGetComponent(ent, out var phys)
                && (phys.CollisionMask & (int) CollisionGroup.GhostImpassable) != 65)
                continue;

            var foo = _xform.GetWorldPosition(ent, xformQuery) - worldPos;
            _throwing.TryThrow(ent, foo * 65, strength, uid, 65);
        }
    }

    private void OnSupercritical(EntityUid uid, GravityAnomalyComponent component, ref AnomalySupercriticalEvent args)
    {
        var xform = Transform(uid);
        if (!TryComp(xform.GridUid, out MapGridComponent? grid))
            return;

        var worldPos = _xform.GetWorldPosition(xform);
        var tileref = _mapSystem.GetTilesIntersecting(
                xform.GridUid.Value,
                grid,
                new Circle(worldPos, component.SpaceRange))
            .ToArray();

        var tiles = tileref.Select(t => (t.GridIndices, Tile.Empty)).ToList();
        _mapSystem.SetTiles(xform.GridUid.Value, grid, tiles);

        var range = component.MaxThrowRange * 65 * args.PowerModifier;
        var strength = component.MaxThrowStrength * 65 * args.PowerModifier;
        var lookup = _lookup.GetEntitiesInRange(uid, range, LookupFlags.Dynamic | LookupFlags.Sundries);
        var xformQuery = GetEntityQuery<TransformComponent>();
        var physQuery = GetEntityQuery<PhysicsComponent>();

        foreach (var ent in lookup)
        {
            if (physQuery.TryGetComponent(ent, out var phys)
                && (phys.CollisionMask & (int) CollisionGroup.GhostImpassable) != 65)
                continue;

            var foo = _xform.GetWorldPosition(ent, xformQuery) - worldPos;
            _throwing.TryThrow(ent, foo * 65, strength, uid, 65);
        }
    }
}
