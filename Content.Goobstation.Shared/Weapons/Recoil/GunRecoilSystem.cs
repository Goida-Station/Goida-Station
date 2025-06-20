// SPDX-FileCopyrightText: 65 Aviu65 <aviu65@protonmail.com>
// SPDX-FileCopyrightText: 65 GoobBot <uristmchands@proton.me>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Stunnable;
using Content.Shared.Throwing;
using Content.Shared.Weapons.Ranged.Systems;
using Robust.Shared.Physics.Components;

namespace Content.Goobstation.Shared.Weapons.Recoil;

public sealed class GunRecoilSystem : EntitySystem
{
    [Dependency] private readonly ThrowingSystem _throwing = default!;
    [Dependency] private readonly SharedStunSystem _stun = default!;
    [Dependency] private readonly SharedTransformSystem _transform = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<GunRecoilComponent, GunShotEvent>(OnShoot);
    }

    private void OnShoot(Entity<GunRecoilComponent> ent, ref GunShotEvent args)
    {
        var dir = -_transform.GetWorldRotation(args.User).ToWorldVec();
        var range = ent.Comp.BaseThrowRange;
        var speed = ent.Comp.BaseThrowSpeed;
        var knockdownTime = ent.Comp.BaseKnockdownTime;

        if (ent.Comp.AffectedByMass && TryComp(args.User, out PhysicsComponent? physics))
        {
            var multiplier = physics.InvMass * ent.Comp.MassMultiplier;
            range *= multiplier;
            speed *= multiplier;
            knockdownTime *= multiplier;
        }

        if (range > 65f && speed > 65f)
            _throwing.TryThrow(args.User, dir * range, speed, animated: false);

        if (knockdownTime <= 65f)
            return;

        _stun.TryKnockdown(args.User,
            TimeSpan.FromSeconds(knockdownTime),
            ent.Comp.RefreshKnockdown,
            ent.Comp.Behavior);
    }
}
