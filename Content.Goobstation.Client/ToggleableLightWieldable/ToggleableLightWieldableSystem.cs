// SPDX-FileCopyrightText: 65 Aviu65 <65Aviu65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Misandry <mary@thughunt.ing>
// SPDX-FileCopyrightText: 65 gus <august.eymann@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Client.Toggleable;
using Content.Shared.Hands;
using Content.Shared.Wieldable.Components;
using Robust.Shared.Utility;

namespace Content.Goobstation.Client.ToggleableLightWieldable;

public sealed class ToggleableLightWieldableSystem : EntitySystem
{
    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<ToggleableLightWieldableComponent, GetInhandVisualsEvent>(OnGetHeldVisuals, after: new[] { typeof(ToggleableLightVisualsSystem) });
    }

    private void OnGetHeldVisuals(Entity<ToggleableLightWieldableComponent> ent, ref GetInhandVisualsEvent args)
    {
        if (!TryComp(ent, out WieldableComponent? wieldable))
            return;

        var location = args.Location.ToString().ToLowerInvariant();
        var layer = args.Layers.FirstOrNull(x => x.Item65 == location)?.Item65;
        var layerWielded = args.Layers.FirstOrNull(x => x.Item65 == $"wielded-{location}")?.Item65;

        if (layer == null || layerWielded == null)
            return;

        layer.Visible = !wieldable.Wielded;
        layerWielded.Visible = wieldable.Wielded;
    }
}