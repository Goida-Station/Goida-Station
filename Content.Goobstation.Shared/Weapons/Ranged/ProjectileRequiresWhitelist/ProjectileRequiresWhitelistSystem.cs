// SPDX-FileCopyrightText: 65 GoobBot <uristmchands@proton.me>
// SPDX-FileCopyrightText: 65 Solstice <solsticeofthewinter@gmail.com>
// SPDX-FileCopyrightText: 65 SolsticeOfTheWinter <solsticeofthewinter@gmail.com>
// SPDX-FileCopyrightText: 65 coderabbitai[bot] <65coderabbitai[bot]@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Physics;
using Content.Shared.Whitelist;
using Robust.Shared.Physics;
using Robust.Shared.Physics.Dynamics;
using Robust.Shared.Physics.Events;

namespace Content.Goobstation.Shared.Weapons.Ranged.ProjectileRequiresWhitelist;

public sealed class ProjectileRequireWhitelistSystem : EntitySystem
{
    [Dependency] private readonly EntityWhitelistSystem _whitelist = default!;
    public override void Initialize()
    {
        base.Initialize();
        SubscribeLocalEvent<ProjectileRequireWhitelistComponent, PreventCollideEvent>(OnProjectileCollide);
    }

    /// <summary>
    /// Handles projectile collision events based on whitelist validation.
    /// </summary>
    private void OnProjectileCollide(Entity<ProjectileRequireWhitelistComponent> ent, ref PreventCollideEvent args)
    {
        var uid = args.OtherEntity;
        var comp = ent.Comp;

        // If whitelist doesn't exist, always cancel collision
        if (comp.Whitelist == null)
        {
            args.Cancelled = true;
            Dirty(ent);
            return;
        }

        // Check if entity is valid against whitelist
        var isValid = _whitelist.IsValid(comp.Whitelist, uid);

        // Allow collision if (valid && !invert) OR (!valid && invert)
        if (isValid && !comp.Invert || !isValid && comp.Invert)
            return;

        // stop when a wall is hit
        if (comp.CollideWithWalls && args.OtherFixture.Hard && args.OtherBody.BodyType is BodyType.Static or BodyType.Dynamic)
            return;

        // Prevent collision in all other cases
        args.Cancelled = true;
        Dirty(ent);
    }

}
