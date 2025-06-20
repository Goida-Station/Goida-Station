// SPDX-FileCopyrightText: 65 GoobBot <uristmchands@proton.me>
// SPDX-FileCopyrightText: 65 SolsticeOfTheWinter <solsticeofthewinter@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.IdentityManagement.Components;
using Content.Shared.Inventory;

namespace Content.Goobstation.Shared.Chemistry;

public sealed partial class GoobVaporSystem : EntitySystem
{
    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<IdentityBlockerComponent, InventoryRelayedEvent<VaporCheckEyeProtectionEvent>>(OnCheckProtectionEvent);
    }

    private void OnCheckProtectionEvent(Entity<IdentityBlockerComponent> ent, ref InventoryRelayedEvent<VaporCheckEyeProtectionEvent> args)
    {
        if (ent.Comp.Coverage is IdentityBlockerCoverage.MOUTH or IdentityBlockerCoverage.NONE || !ent.Comp.Enabled)
            return;

        args.Args.Protected = true;
    }

}

