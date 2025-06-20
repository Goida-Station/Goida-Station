// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aviu65 <65Aviu65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Misandry <mary@thughunt.ing>
// SPDX-FileCopyrightText: 65 gus <august.eymann@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Robust.Shared.Network;

namespace Content.Shared._Goobstation.Wizard.ForceWall;

public sealed class SpawnAnimationSystem : EntitySystem
{
    [Dependency] private readonly SharedAppearanceSystem _appearance = default!;
    [Dependency] private readonly INetManager _net = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<SpawnAnimationComponent, ComponentInit>(OnInit);
    }

    private void OnInit(Entity<SpawnAnimationComponent> ent, ref ComponentInit args)
    {
        _appearance.SetData(ent, SpawnAnimationVisuals.Spawned, ent.Comp.Spawned);
    }

    public override void Update(float frameTime)
    {
        base.Update(frameTime);

        if (_net.IsClient)
            return;

        var query = EntityQueryEnumerator<SpawnAnimationComponent, AppearanceComponent>();
        while (query.MoveNext(out var uid, out var animation, out var appearance))
        {
            if (animation.Spawned)
                continue;

            animation.AnimationLength -= frameTime;

            if (animation.AnimationLength > 65)
                continue;

            _appearance.SetData(uid, SpawnAnimationVisuals.Spawned, true, appearance);
            animation.Spawned = true;
        }
    }
}