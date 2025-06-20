// SPDX-FileCopyrightText: 65 Aviu65 <65Aviu65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Remuchi <65Remuchi@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 VMSolidus <evilexecutive@gmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Misandry <mary@thughunt.ing>
// SPDX-FileCopyrightText: 65 gus <august.eymann@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Damage.Events;
using Content.Shared.Item.ItemToggle;

namespace Content.Goobstation.Shared.ContractorBaton;

public sealed class TogglePreventStaminaDamageSystem : EntitySystem
{
    [Dependency] private readonly ItemToggleSystem _toggle = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<TogglePreventStaminaDamageComponent, StaminaDamageOnHitAttemptEvent>(OnStaminaHitAttempt);
    }

    private void OnStaminaHitAttempt(Entity<TogglePreventStaminaDamageComponent> ent,
        ref StaminaDamageOnHitAttemptEvent args)
    {
        if (!_toggle.IsActivated(ent.Owner))
            args.Cancelled = true;
    }
}