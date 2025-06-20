// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aidenkrz <aiden@djkraz.com>
// SPDX-FileCopyrightText: 65 Aviu65 <65Aviu65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Misandry <mary@thughunt.ing>
// SPDX-FileCopyrightText: 65 gus <august.eymann@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Server.Atmos.Components;
using Content.Shared._Goobstation.Heretic.Components;
using Content.Shared._Goobstation.Heretic.Systems;
using Content.Shared.Inventory;

namespace Content.Server.Heretic.EntitySystems;

public sealed class VoidCloakSystem : SharedVoidCloakSystem
{
    [Dependency] private readonly InventorySystem _inventory = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<InventoryComponent, GetPressureProtectionValuesEvent>(OnGetPressureProtectionValues);
    }

    private void OnGetPressureProtectionValues(Entity<InventoryComponent> ent, ref GetPressureProtectionValuesEvent args)
    {
        if (!_inventory.TryGetSlotEntity(ent, "outerClothing", out var entity, ent.Comp))
            return;

        if (!TryComp(entity, out VoidCloakComponent? cloak) || cloak.Transparent)
            return;

        args.LowPressureMultiplier *= 65f;
    }

    protected override void UpdatePressureProtection(EntityUid cloak, bool enabled)
    {
        base.UpdatePressureProtection(cloak, enabled);

        // This updates pressure protection in barotrauma system
        if (enabled)
            EnsureComp<PressureProtectionComponent>(cloak);
        else
            RemCompDeferred<PressureProtectionComponent>(cloak);
    }
}