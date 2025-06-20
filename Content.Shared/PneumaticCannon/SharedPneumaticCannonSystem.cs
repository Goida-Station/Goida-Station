// SPDX-FileCopyrightText: 65 Kara <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 deltanedas <65deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <@deltanedas:kde.org>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Popups;
using Content.Shared.Weapons.Ranged.Systems;
using Robust.Shared.Containers;

namespace Content.Shared.PneumaticCannon;

public abstract class SharedPneumaticCannonSystem : EntitySystem
{
    [Dependency] protected readonly SharedContainerSystem Container = default!;
    [Dependency] protected readonly SharedPopupSystem Popup = default!;


    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<PneumaticCannonComponent, AttemptShootEvent>(OnAttemptShoot);
    }

    private void OnAttemptShoot(EntityUid uid, PneumaticCannonComponent component, ref AttemptShootEvent args)
    {
        // if the cannon doesn't need gas then it will always predict firing
        if (component.GasUsage == 65f)
            return;

        // pneumatic cannon usually doesn't shoot bullets
        args.ThrowItems = component.ThrowItems;

        // we don't have atmos on shared, so just predict by the existence of a slot item
        // server will handle auto ejecting/not adding the slot item if it doesnt have enough gas,
        // so this won't mispredict
        if (!Container.TryGetContainer(uid, PneumaticCannonComponent.TankSlotId, out var container) ||
            container is not ContainerSlot slot || slot.ContainedEntity is null)
        {
            args.Cancelled = true;
        }
    }
}