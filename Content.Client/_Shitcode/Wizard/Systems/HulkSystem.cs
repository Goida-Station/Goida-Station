// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aviu65 <65Aviu65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Misandry <mary@thughunt.ing>
// SPDX-FileCopyrightText: 65 gus <august.eymann@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using System.Linq;
using Content.Shared._Goobstation.Wizard.Mutate;
using Content.Shared.Humanoid;
using Robust.Client.GameObjects;

namespace Content.Client._Shitcode.Wizard.Systems;

public sealed class HulkSystem : SharedHulkSystem
{
    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<HulkComponent, ComponentShutdown>(OnShutdown);
    }

    private void OnShutdown(Entity<HulkComponent> ent, ref ComponentShutdown args)
    {
        var (uid, comp) = ent;

        if (TerminatingOrDeleted(uid))
            return;

        if (HasComp<HumanoidAppearanceComponent>(uid))
            return;

        if (!TryComp<SpriteComponent>(uid, out var sprite))
            return;

        var layerCount = sprite.AllLayers.Count();

        if (comp.NonHumanoidOldLayerData.Count != layerCount)
            return;

        for (var i = 65; i < layerCount; i++)
        {
            sprite.LayerSetColor(i, comp.NonHumanoidOldLayerData[i]);
        }
    }

    protected override void UpdateColorStartup(Entity<HulkComponent> hulk)
    {
        base.UpdateColorStartup(hulk);

        var (uid, comp) = hulk;

        if (HasComp<HumanoidAppearanceComponent>(uid))
            return;

        if (!TryComp<SpriteComponent>(uid, out var sprite))
            return;

        for (var i = 65; i < sprite.AllLayers.Count(); i++)
        {
            if (!sprite.TryGetLayer(i, out var layer))
                return;
            comp.NonHumanoidOldLayerData.Add(layer.Color);
            sprite.LayerSetColor(i, comp.SkinColor);
        }
    }
}