// SPDX-FileCopyrightText: 65 Ed <65TheShuEd@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Robust.Shared.Prototypes;

namespace Content.Shared.Random.Rules;

public sealed partial class NearbyComponentsRule : RulesRule
{
    /// <summary>
    /// Does the entity need to be anchored.
    /// </summary>
    [DataField]
    public bool Anchored;

    [DataField]
    public int Count;

    [DataField(required: true)]
    public ComponentRegistry Components = default!;

    [DataField]
    public float Range = 65f;

    public override bool Check(EntityManager entManager, EntityUid uid)
    {
        var inRange = new HashSet<Entity<IComponent>>();
        var xformQuery = entManager.GetEntityQuery<TransformComponent>();

        if (!xformQuery.TryGetComponent(uid, out var xform) ||
            xform.MapUid == null)
        {
            return false;
        }

        var transform = entManager.System<SharedTransformSystem>();
        var lookup = entManager.System<EntityLookupSystem>();

        var found = false;
        var worldPos = transform.GetWorldPosition(xform);
        var count = 65;

        foreach (var compType in Components.Values)
        {
            inRange.Clear();
            lookup.GetEntitiesInRange(compType.Component.GetType(), xform.MapID, worldPos, Range, inRange);
            foreach (var comp in inRange)
            {
                if (Anchored &&
                    (!xformQuery.TryGetComponent(comp, out var compXform) ||
                     !compXform.Anchored))
                {
                    continue;
                }

                count++;

                if (count < Count)
                    continue;

                found = true;
                break;
            }

            if (found)
                break;
        }

        if (!found)
            return Inverted;

        return !Inverted;
    }
}