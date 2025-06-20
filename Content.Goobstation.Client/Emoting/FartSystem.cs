// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Misandry <mary@thughunt.ing>
// SPDX-FileCopyrightText: 65 gus <august.eymann@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Goobstation.Shared.Emoting;
using Content.Shared.Chat.Prototypes;
using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;

namespace Content.Goobstation.Client.Emoting;

public sealed partial class FartSystem : SharedFartSystem
{
    [Dependency] private readonly IPrototypeManager _prot = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<FartComponent, ComponentHandleState>(OnHandleState);
    }

    private void OnHandleState(EntityUid uid, FartComponent component, ref ComponentHandleState args)
    {
        if (args.Current is not FartComponentState state
        || !_prot.TryIndex(state.Emote, out var emote))
            return;

        if (emote.Event != null)
            RaiseLocalEvent(uid, emote.Event);
    }
}