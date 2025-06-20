// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aviu65 <65Aviu65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Misandry <mary@thughunt.ing>
// SPDX-FileCopyrightText: 65 Spatison <65Spatison@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 gus <august.eymann@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared._Goobstation.Heretic.Components;
using Content.Shared._Goobstation.Wizard;
using Content.Shared.Inventory.Events;

namespace Content.Shared.Heretic.Systems;

public sealed class HereticClothingSystem : EntitySystem
{
    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<HereticClothingComponent, BeingEquippedAttemptEvent>(OnEquipAttempt);
    }

    private void OnEquipAttempt(Entity<HereticClothingComponent> ent, ref BeingEquippedAttemptEvent args)
    {
        if (IsTargetValid(args.EquipTarget) && (args.EquipTarget == args.Equipee || IsTargetValid(args.Equipee)))
            return;

        args.Cancel();
        args.Reason = Loc.GetString("heretic-clothing-component-fail");
    }

    private bool IsTargetValid(EntityUid target)
    {
        return HasComp<HereticComponent>(target) || HasComp<GhoulComponent>(target) ||
               HasComp<WizardComponent>(target) || HasComp<ApprenticeComponent>(target);
    }
}