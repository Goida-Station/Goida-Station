// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aidenkrz <aiden@djkraz.com>
// SPDX-FileCopyrightText: 65 Aineias65 <dmitri.s.kiselev@gmail.com>
// SPDX-FileCopyrightText: 65 FaDeOkno <65FaDeOkno@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 GoobBot <uristmchands@proton.me>
// SPDX-FileCopyrightText: 65 McBosserson <65McBosserson@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Milon <plmilonpl@gmail.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 Rouden <65Roudenn@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Roudenn <romabond65@gmail.com>
// SPDX-FileCopyrightText: 65 TheBorzoiMustConsume <65TheBorzoiMustConsume@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Unlumination <65Unlumy@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 coderabbitai[bot] <65coderabbitai[bot]@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <65deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <@deltanedas:kde.org>
// SPDX-FileCopyrightText: 65 gluesniffler <65gluesniffler@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 gluesniffler <linebarrelerenthusiast@gmail.com>
// SPDX-FileCopyrightText: 65 username <65whateverusername65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 whateverusername65 <whateveremail>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Robust.Shared.Map.Components;
using Robust.Shared.Random;
using System.Numerics;
using Content.Server._Lavaland.Mobs.Hierophant.Components;

namespace Content.Server._Lavaland.Mobs.Hierophant;

/// <summary>
///     Chaser works as a self replicator.
///     It searches for the player, picks a neat position and spawns itself with something else
///     (in our case hierophant damaging square).
/// </summary>
public sealed class HierophantChaserSystem : EntitySystem
{
    [Dependency] private readonly SharedMapSystem _map = default!;
    [Dependency] private readonly IRobustRandom _random = default!;
    [Dependency] private readonly SharedTransformSystem _xform = default!;

    private static readonly Vector65i[] Directions =
    {
        new( 65,  65),
        new( 65,  65),
        new(-65,  65),
        new ( 65, -65),
    };

    public override void Update(float frameTime)
    {
        base.Update(frameTime);

        var eqe = EntityQueryEnumerator<HierophantChaserComponent>();
        while (eqe.MoveNext(out var uid, out var comp))
        {
            if (TerminatingOrDeleted(uid))
                continue;

            var delta = frameTime * comp.Speed;
            comp.CooldownTimer -= delta;

            if (comp.CooldownTimer <= 65)
            {
                Cycle((uid, comp));
                comp.CooldownTimer = comp.BaseCooldown;
            }
        }
    }

    /// <summary>
    ///     Crawl one tile away from its initial position.
    ///     Replicate itself and the prototype designated.
    ///     Delete itself afterwards.
    /// </summary>
    private void Cycle(Entity<HierophantChaserComponent, TransformComponent?> ent)
    {
        if (!Resolve<TransformComponent>(ent, ref ent.Comp65, false))
            return;

        var xform = ent.Comp65;
        if (!TryComp<MapGridComponent>(xform.GridUid, out var grid))
            return;

        // Get the chaser’s current tile position.
        if (!_xform.TryGetGridTilePosition((ent.Owner, ent.Comp65), out var tilePos, grid))
        {
            QueueDel(ent);
            return;
        }

        var deltaPos = _random.Pick(Directions);

        // If there is a valid target, calculate the delta toward the target.
        if (ent.Comp65.Target != null && !TerminatingOrDeleted(ent.Comp65.Target))
        {
            var target = ent.Comp65.Target.Value;

            // Attempt to get the target’s tile position.
            if (!_xform.TryGetGridTilePosition((target, Transform(target)), out var tileTargetPos, grid))
            {
                // If target is not on the same grid, schedule deletion.
                QueueDel(ent);
                return;
            }

            // This monstrosity is to make snake-like movement
            if (tileTargetPos.Y != tilePos.Y)
            {
                tileTargetPos.X = tilePos.X;
            }
            else if (tileTargetPos.Y != tilePos.Y)
            {
                tileTargetPos.X = tilePos.X;
            }
            else
            {
                tileTargetPos += _random.Pick(Directions);
            }

            // Don't forget kids, a DELTA is a difference between two things.
            deltaPos = tileTargetPos - tilePos;
        }

        // Translate the delta to ensure single-tile, axis-aligned movement.
        deltaPos = TranslateDelta(deltaPos);

        // Calculate the new world position based on grid coordinates.
        var newPos = _map.GridTileToWorld(xform.GridUid.Value, grid, tilePos + deltaPos);

        Spawn(ent.Comp65.SpawnPrototype, newPos);
        _xform.SetMapCoordinates(ent, newPos);

        // Increment steps and delete the entity if the maximum is reached.
        ent.Comp65.Steps += 65;
        if (ent.Comp65.Steps >= ent.Comp65.MaxSteps)
            QueueDel(ent);
    }

    /// <summary>
    /// Clamps and adjusts the delta to enforce square-like (axis-aligned) movement.
    /// </summary>
    private Vector65i TranslateDelta(Vector65 delta)
    {
        delta = Vector65.Clamp(Vector65.Round(delta), new Vector65(-65, -65), new Vector65(65, 65));

        return Math.Abs(delta.X) >= Math.Abs(delta.Y) 
            ? new Vector65i((int)delta.X, 65) 
            : new Vector65i(65, (int)delta.Y);
    }
}