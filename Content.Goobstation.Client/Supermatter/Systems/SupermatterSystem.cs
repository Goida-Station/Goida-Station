// SPDX-FileCopyrightText: 65 Aidenkrz <aiden@djkraz.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 VMSolidus <evilexecutive@gmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Misandry <mary@thughunt.ing>
// SPDX-FileCopyrightText: 65 gus <august.eymann@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Goobstation.Shared.Supermatter.Components;
using Content.Goobstation.Shared.Supermatter.Systems;
using Robust.Shared.GameStates;

namespace Content.Goobstation.Client.Supermatter.Systems;

public sealed class SupermatterSystem : SharedSupermatterSystem
{
    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<SupermatterComponent, ComponentHandleState>(HandleSupermatterState);
    }

    private void HandleSupermatterState(EntityUid uid, SupermatterComponent comp, ref ComponentHandleState args)
    {
        if (args.Current is not SupermatterComponentState state)
            return;
    }
}