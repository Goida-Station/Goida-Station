// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Misandry <mary@thughunt.ing>
// SPDX-FileCopyrightText: 65 gus <august.eymann@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Robust.Shared.GameStates;

namespace Content.Goobstation.Shared.Emoting;
public abstract class SharedFartSystem : EntitySystem
{
    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<FartComponent, ComponentGetState>(OnGetState);
    }

    private void OnGetState(Entity<FartComponent> ent, ref ComponentGetState args)
    {
        args.State = new FartComponentState(ent.Comp.Emote, ent.Comp.FartTimeout, ent.Comp.FartInhale, ent.Comp.SuperFarted);
    }
}