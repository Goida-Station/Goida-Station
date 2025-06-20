// SPDX-FileCopyrightText: 65 deltanedas <65deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <@deltanedas:kde.org>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <aiden@djkraz.com>
// SPDX-FileCopyrightText: 65 BombasterDS <65BombasterDS@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 BombasterDS <deniskaporoshok@gmail.com>
// SPDX-FileCopyrightText: 65 BombasterDS65 <shvalovdenis.workmail@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Item.ItemToggle.Components;

namespace Content.Shared.Item.ItemToggle;

/// <summary>
/// Handles <see cref="ComponentTogglerComponent"/> component manipulation.
/// </summary>
public sealed class ComponentTogglerSystem : EntitySystem
{
    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<ComponentTogglerComponent, ItemToggledEvent>(OnToggled);
    }

    private void OnToggled(Entity<ComponentTogglerComponent> ent, ref ItemToggledEvent args)
    {
        ToggleComponent(ent, args.Activated);
    }

    // Goobstation - Make this system more flexible
    public void ToggleComponent(EntityUid uid, bool activate)
    {
        if (!TryComp<ComponentTogglerComponent>(uid, out var component))
            return;

        var target = component.Parent ? Transform(uid).ParentUid : uid;

        if (activate)
            EntityManager.AddComponents(target, component.Components);
        else
            EntityManager.RemoveComponents(target, component.RemoveComponents ?? component.Components);
    }
}