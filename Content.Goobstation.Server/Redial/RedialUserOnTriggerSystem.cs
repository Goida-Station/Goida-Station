// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <aiden@djkraz.com>
// SPDX-FileCopyrightText: 65 Misandry <mary@thughunt.ing>
// SPDX-FileCopyrightText: 65 gus <august.eymann@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Server.Explosion.EntitySystems;
using Robust.Shared.GameObjects;
using Robust.Shared.IoC;
using Robust.Shared.Player;

namespace Content.Goobstation.Server.Redial;

public sealed class RedialUserOnTriggerSystem : EntitySystem
{
    [Dependency] private readonly RedialManager _redial = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<RedialUserOnTriggerComponent, TriggerEvent>(OnTrigger);
    }

    private void OnTrigger(EntityUid uid, RedialUserOnTriggerComponent component, TriggerEvent args)
    {
        if (!TryComp(args.User, out ActorComponent? actor) || component.Address == string.Empty)
            return;

        _redial.Redial(actor.PlayerSession.Channel, component.Address);

        args.Handled = true;
    }
}