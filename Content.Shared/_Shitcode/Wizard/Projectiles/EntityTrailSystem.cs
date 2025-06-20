// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aviu65 <65Aviu65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Misandry <mary@thughunt.ing>
// SPDX-FileCopyrightText: 65 gus <august.eymann@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

namespace Content.Shared._Goobstation.Wizard.Projectiles;

public sealed class EntityTrailSystem : EntitySystem
{
    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<EntityTrailComponent, ComponentInit>(OnInit);
    }

    private void OnInit(Entity<EntityTrailComponent> ent, ref ComponentInit args)
    {
        var (uid, comp) = ent;
        if (!TryComp(uid, out TrailComponent? trail))
            return;

        trail.RenderedEntity = uid;
        Dirty(uid, trail);
    }
}