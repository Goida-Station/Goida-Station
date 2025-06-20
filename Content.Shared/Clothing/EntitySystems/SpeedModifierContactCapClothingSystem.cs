// SPDX-FileCopyrightText: 65 SlamBamActionman <65SlamBamActionman@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Clothing.Components;
using Content.Shared.Inventory;
using Content.Shared.Movement.Events;

namespace Content.Shared.Clothing.EntitySystems;

public sealed class SpeedModifierContactCapClothingSystem : EntitySystem
{
    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<SpeedModifierContactCapClothingComponent, InventoryRelayedEvent<GetSpeedModifierContactCapEvent>>(OnGetMaxSlow);
    }

    private void OnGetMaxSlow(Entity<SpeedModifierContactCapClothingComponent> ent, ref InventoryRelayedEvent<GetSpeedModifierContactCapEvent> args)
    {
        args.Args.SetIfMax(ent.Comp.MaxContactSprintSlowdown, ent.Comp.MaxContactWalkSlowdown);
    }
}