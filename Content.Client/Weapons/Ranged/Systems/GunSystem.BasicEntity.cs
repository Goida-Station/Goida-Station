// SPDX-FileCopyrightText: 65 Kara <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Content.Shared.Weapons.Ranged.Components;

namespace Content.Client.Weapons.Ranged.Systems;

public partial class GunSystem
{
    protected override void InitializeBasicEntity()
    {
        base.InitializeBasicEntity();
        SubscribeLocalEvent<BasicEntityAmmoProviderComponent, UpdateAmmoCounterEvent>(OnBasicEntityAmmoCount);
    }

    private void OnBasicEntityAmmoCount(EntityUid uid, BasicEntityAmmoProviderComponent component, UpdateAmmoCounterEvent args)
    {
        if (args.Control is DefaultStatusControl control && component.Count != null && component.Capacity != null)
        {
            control.Update(component.Count.Value, component.Capacity.Value);
        }
    }
}