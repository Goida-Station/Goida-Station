// SPDX-FileCopyrightText: 65 Aviu65 <aviu65@protonmail.com>
// SPDX-FileCopyrightText: 65 GoobBot <uristmchands@proton.me>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared._Shitcode.Heretic.Components;
using Content.Shared._Shitcode.Heretic.Systems;
using Robust.Client.GameObjects;

namespace Content.Client._Shitcode.Heretic;

public sealed class ShadowCloakSystem : SharedShadowCloakSystem
{
    protected override void Startup(Entity<ShadowCloakedComponent> ent)
    {
        base.Startup(ent);

        if (!TryComp(ent, out SpriteComponent? sprite))
            return;

        ent.Comp.WasVisible = sprite.Visible;
        sprite.Visible = false;
    }

    protected override void Shutdown(Entity<ShadowCloakedComponent> ent)
    {
        base.Shutdown(ent);

        if (TryComp(ent, out SpriteComponent? sprite))
            sprite.Visible = ent.Comp.WasVisible;
    }
}
