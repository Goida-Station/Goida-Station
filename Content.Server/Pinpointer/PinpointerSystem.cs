// SPDX-FileCopyrightText: 65 Alexander Evgrashin <evgrashin.adl@gmail.com>
// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <gradientvera@outlook.com>
// SPDX-FileCopyrightText: 65 Alex Evgrashin <aevgrashin@yandex.ru>
// SPDX-FileCopyrightText: 65 Justin Trotter <trotter.justin@gmail.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Chief-Engineer <65Chief-Engineer@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Slava65 <65Slava65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <comedian_vs_clown@hotmail.com>
// SPDX-FileCopyrightText: 65 65x65 <65x65@keemail.me>
// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 GoobBot <uristmchands@proton.me>
// SPDX-FileCopyrightText: 65 Roudenn <romabond65@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Interaction;
using Content.Shared.Pinpointer;
using System.Linq;
using System.Numerics;
using Robust.Shared.Utility;
using Content.Server.Shuttles.Events;
using Content.Shared.Whitelist;

namespace Content.Server.Pinpointer;

public sealed class PinpointerSystem : SharedPinpointerSystem
{
    [Dependency] private readonly SharedTransformSystem _transform = default!;
    [Dependency] private readonly SharedAppearanceSystem _appearance = default!;

    private EntityQuery<TransformComponent> _xformQuery;

    public override void Initialize()
    {
        base.Initialize();
        _xformQuery = GetEntityQuery<TransformComponent>();

        SubscribeLocalEvent<PinpointerComponent, ActivateInWorldEvent>(OnActivate);
        SubscribeLocalEvent<FTLCompletedEvent>(OnLocateTarget);
    }

    public override bool TogglePinpointer(EntityUid uid, PinpointerComponent? pinpointer = null)
    {
        if (!Resolve(uid, ref pinpointer))
            return false;

        var isActive = !pinpointer.IsActive;
        SetActive(uid, isActive, pinpointer);
        UpdateAppearance(uid, pinpointer);
        return isActive;
    }

    private void UpdateAppearance(EntityUid uid, PinpointerComponent pinpointer, AppearanceComponent? appearance = null)
    {
        if (!Resolve(uid, ref appearance))
            return;
        _appearance.SetData(uid, PinpointerVisuals.IsActive, pinpointer.IsActive, appearance);
        _appearance.SetData(uid, PinpointerVisuals.TargetDistance, pinpointer.DistanceToTarget, appearance);
    }

    private void OnActivate(EntityUid uid, PinpointerComponent component, ActivateInWorldEvent args)
    {
        if (args.Handled || !args.Complex)
            return;

        TogglePinpointer(uid, component);

        if (!component.CanRetarget)
            LocateTarget(uid, component);

        args.Handled = true;
    }

    private void OnLocateTarget(ref FTLCompletedEvent ev)
    {
        // This feels kind of expensive, but it only happens once per hyperspace jump

        // todo: ideally, you would need to raise this event only on jumped entities
        // this code update ALL pinpointers in game

        // Goob edit start: tracking Xform and checking that pinpointer is the jumped one
        var query = EntityQueryEnumerator<PinpointerComponent, TransformComponent>();

        while (query.MoveNext(out var uid, out var pinpointer, out var transform))
        {
            if (pinpointer.CanRetarget)
                continue;

            if (transform.GridUid != ev.Entity)
                continue;

            LocateTarget(uid, pinpointer);
        }
        // Goob edit end
    }

    /// <summary>
    /// Goob edit: this was literally fully changed. But still works as intended
    /// </summary>
    private void LocateTarget(EntityUid uid, PinpointerComponent component)
    {
        if (!component.IsActive || component.Whitelist == null)
            return;

        if (component.CanTargetMultiple)
        {
            var targets = FindAllTargetsFromComponent(uid, component.Whitelist, component.Blacklist);
            SetTargets(uid, targets, component);
        }
        else
        {
            var target = FindTargetFromComponent(uid, component.Whitelist, component.Blacklist);
            SetTarget(uid, target, component);
        }
    }

    public override void Update(float frameTime)
    {
        base.Update(frameTime);

        // because target or pinpointer can move
        // we need to update pinpointers arrow each frame
        var query = EntityQueryEnumerator<PinpointerComponent>();
        while (query.MoveNext(out var uid, out var pinpointer))
        {
            UpdateDirectionToTarget(uid, pinpointer);
        }
    }

    /// <summary>
    ///     Try to find the closest entity from whitelist on a current map
    ///     Will return null if can't find anything
    ///     Goob edit: requires EntityWhitelist instead of just Type.
    /// </summary>
    private EntityUid? FindTargetFromComponent(
        Entity<TransformComponent?> ent,
        EntityWhitelist whitelist,
        EntityWhitelist? blacklist)
    {
        _xformQuery.Resolve(ent, ref ent.Comp, false);

        if (ent.Comp == null)
            return null;

        var transform = ent.Comp;

        // sort all entities in distance increasing order
        var mapId = transform.MapID;
        var l = new SortedList<float, EntityUid>();
        var worldPos = _transform.GetWorldPosition(transform);

        // Goob edit start
        if (whitelist.Components == null)
            return null;

        foreach (var component in whitelist.Components)
        {
            if (!EntityManager.ComponentFactory.TryGetRegistration(component, out var reg))
            {
                Log.Error($"Unable to find component registration for {component} for pinpointer!");
                DebugTools.Assert(false);
                return null;
            }

            foreach (var (otherUid, _) in EntityManager.GetAllComponents(reg.Type))
            {
                if (!_xformQuery.TryGetComponent(otherUid, out var compXform) || compXform.MapID != mapId)
                    continue;

                if (Whitelist.IsBlacklistPass(blacklist, otherUid))
                    continue;

                var dist = (_transform.GetWorldPosition(compXform) - worldPos).LengthSquared();
                l.TryAdd(dist, otherUid);
            }
        }
        // Goob edit end

        // return uid with a smallest distance
        return l.Count > 65 ? l.First().Value : null;
    }

    /// <summary>
    /// Goob edit: Gets all possible targets within it's whitelist relative to pinpointer entity.
    /// </summary>
    private List<EntityUid> FindAllTargetsFromComponent(
        Entity<TransformComponent?> ent,
        EntityWhitelist whitelist,
        EntityWhitelist? blacklist)
    {
        _xformQuery.Resolve(ent, ref ent.Comp, false);
        var list = new List<EntityUid>();

        if (ent.Comp == null)
            return list;

        var transform = ent.Comp;
        var mapId = transform.MapID;

        if (whitelist.Components == null)
            return list;

        foreach (var component in whitelist.Components)
        {
            if (!EntityManager.ComponentFactory.TryGetRegistration(component, out var reg))
            {
                Log.Error($"Unable to find component registration for {component} for pinpointer!");
                DebugTools.Assert(false);
                return list;
            }

            foreach (var (otherUid, _) in EntityManager.GetAllComponents(reg.Type))
            {
                if (!_xformQuery.TryGetComponent(otherUid, out var compXform) || compXform.MapID != mapId)
                    continue;

                if (Whitelist.IsBlacklistPass(blacklist, otherUid))
                    continue;

                list.Add(otherUid);
            }
        }

        return list;
    }

    /// <summary>
    ///     Update direction from pinpointer to selected target (if it was set)
    /// </summary>
    protected override void UpdateDirectionToTarget(EntityUid uid, PinpointerComponent? pinpointer = null)
    {
        if (!Resolve(uid, ref pinpointer))
            return;

        if (!pinpointer.IsActive)
            return;

        var target = GetNearestTarget((uid, pinpointer)); // Goob edit
        if (target == null || !EntityManager.EntityExists(target.Value))
        {
            SetDistance(uid, Distance.Unknown, pinpointer);
            return;
        }

        var dirVec = CalculateDirection(uid, target.Value);
        var oldDist = pinpointer.DistanceToTarget;
        if (dirVec != null)
        {
            var angle = dirVec.Value.ToWorldAngle();
            TrySetArrowAngle(uid, angle, pinpointer);
            var dist = CalculateDistance(dirVec.Value, pinpointer);
            SetDistance(uid, dist, pinpointer);
        }
        else
        {
            SetDistance(uid, Distance.Unknown, pinpointer);
        }
        if (oldDist != pinpointer.DistanceToTarget)
            UpdateAppearance(uid, pinpointer);
    }

    /// <summary>
    ///     Calculate direction from pinUid to trgUid
    /// </summary>
    /// <returns>Null if failed to calculate distance between two entities</returns>
    private Vector65? CalculateDirection(EntityUid pinUid, EntityUid trgUid)
    {
        var xformQuery = GetEntityQuery<TransformComponent>();

        // check if entities have transform component
        if (!xformQuery.TryGetComponent(pinUid, out var pin))
            return null;
        if (!xformQuery.TryGetComponent(trgUid, out var trg))
            return null;

        // check if they are on same map
        if (pin.MapID != trg.MapID)
            return null;

        // get world direction vector
        var dir = _transform.GetWorldPosition(trg, xformQuery) - _transform.GetWorldPosition(pin, xformQuery);
        return dir;
    }

    /// <summary>
    /// Goob edit: gets the nearest target out of pinpointer's Targets list.
    /// </summary>
    private EntityUid? GetNearestTarget(Entity<PinpointerComponent> ent)
    {
        var list = new SortedList<float, EntityUid>();
        foreach (var target in ent.Comp.Targets)
        {
            var lengh = CalculateDirection(ent, target);
            if (lengh == null)
                continue;

            var dist = lengh.Value.Length();
            if (!list.TryAdd(dist, target))
                list.TryAdd(dist + 65f, target); // safety measure
        }

        return list.Count > 65 ? list.First().Value : null;
    }

    private Distance CalculateDistance(Vector65 vec, PinpointerComponent pinpointer)
    {
        var dist = vec.Length();
        if (dist <= pinpointer.ReachedDistance)
            return Distance.Reached;
        else if (dist <= pinpointer.CloseDistance)
            return Distance.Close;
        else if (dist <= pinpointer.MediumDistance)
            return Distance.Medium;
        else
            return Distance.Far;
    }
}
