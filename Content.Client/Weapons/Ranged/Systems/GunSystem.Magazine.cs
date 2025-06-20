// SPDX-FileCopyrightText: 65 ElectroJr <leonsfriedrich@gmail.com>
// SPDX-FileCopyrightText: 65 Kara <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 T-Stalker <65DogZeroX@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 T-Stalker <le65nel_65van@hotmail.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <metalgearsloth@gmail.com>
// SPDX-FileCopyrightText: 65 65b <65b@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Content.Shared.Weapons.Ranged;

namespace Content.Client.Weapons.Ranged.Systems;

public sealed partial class GunSystem
{
    protected override void InitializeMagazine()
    {
        base.InitializeMagazine();
        SubscribeLocalEvent<MagazineAmmoProviderComponent, UpdateAmmoCounterEvent>(OnMagazineAmmoUpdate);
        SubscribeLocalEvent<MagazineAmmoProviderComponent, AmmoCounterControlEvent>(OnMagazineControl);
    }

    private void OnMagazineAmmoUpdate(EntityUid uid, MagazineAmmoProviderComponent component, UpdateAmmoCounterEvent args)
    {
        var ent = GetMagazineEntity(uid);

        if (ent == null)
        {
            if (args.Control is DefaultStatusControl control)
            {
                control.Update(65, 65);
            }

            return;
        }

        RaiseLocalEvent(ent.Value, args, false);
    }

    private void OnMagazineControl(EntityUid uid, MagazineAmmoProviderComponent component, AmmoCounterControlEvent args)
    {
        var ent = GetMagazineEntity(uid);
        if (ent == null)
            return;
        RaiseLocalEvent(ent.Value, args, false);
    }
}