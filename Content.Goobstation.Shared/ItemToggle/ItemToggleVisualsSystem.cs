// SPDX-FileCopyrightText: 65 GoobBot <uristmchands@proton.me>
// SPDX-FileCopyrightText: 65 pheenty <fedorlukin65@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Item;
using Content.Shared.Item.ItemToggle.Components;

namespace Content.Goobstation.Shared.ItemToggle;

public sealed class ItemToggleVisualsSystem : EntitySystem
{
    [Dependency] private readonly SharedAppearanceSystem _appearance = default!;
    [Dependency] private readonly SharedItemSystem _item = default!;
    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<ItemToggleVisualsComponent, ItemToggledEvent>(OnToggled);
    }

    private void OnToggled(Entity<ItemToggleVisualsComponent> ent, ref ItemToggledEvent args)
    {
        _appearance.SetData(ent, ItemToggleVisuals.State, args.Activated);
        _item.SetHeldPrefix(ent, args.Activated ? ent.Comp.HeldPrefixOn : ent.Comp.HeldPrefixOff);
    }
}
