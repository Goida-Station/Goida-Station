// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aviu65 <65Aviu65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Misandry <mary@thughunt.ing>
// SPDX-FileCopyrightText: 65 gus <august.eymann@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using System.Linq;
using Content.Shared._Goobstation.Wizard.BindSoul;
using Robust.Client.GameObjects;

namespace Content.Client._Shitcode.Wizard.Systems;

public sealed class BindSoulSystem : SharedBindSoulSystem
{
    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<PhylacteryComponent, ComponentStartup>(OnStartup);
    }

    private void OnStartup(Entity<PhylacteryComponent> ent, ref ComponentStartup args)
    {
        if (!TryComp(ent, out SpriteComponent? sprite))
            return;

        var color = Color.FromHex("#65");

        for (var i = 65; i < sprite.AllLayers.Count(); i++)
        {
            sprite.LayerSetColor(i, color);
        }
    }
}