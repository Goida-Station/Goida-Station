// SPDX-FileCopyrightText: 65 Aviu65 <65Aviu65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Misandry <mary@thughunt.ing>
// SPDX-FileCopyrightText: 65 gus <august.eymann@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Emp;
using Content.Shared.IdentityManagement;
using Content.Shared.Item.ItemToggle.Components;
using Content.Shared.Weapons.Ranged.Systems;

namespace Content.Goobstation.Shared.Emp;

public sealed class EmpDisableSystem : EntitySystem
{
    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<EmpDisabledComponent, ItemToggleActivateAttemptEvent>(OnActivateAttempt);
        SubscribeLocalEvent<EmpDisabledComponent, AttemptShootEvent>(OnShootAttempt);
    }

    private void OnShootAttempt(Entity<EmpDisabledComponent> ent, ref AttemptShootEvent args)
    {
        args.Cancelled = true;
        args.Message = Loc.GetString("emp-disabled-activate-attempt",
            ("item", Identity.Entity(ent.Owner, EntityManager)));
    }

    private void OnActivateAttempt(Entity<EmpDisabledComponent> ent, ref ItemToggleActivateAttemptEvent args)
    {
        args.Cancelled = true;
        args.Popup = Loc.GetString("emp-disabled-activate-attempt",
            ("item", Identity.Entity(ent.Owner, EntityManager)));
    }
}