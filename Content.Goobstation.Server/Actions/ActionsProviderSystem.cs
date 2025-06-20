// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 username <65whateverusername65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 whateverusername65 <whateveremail>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Misandry <mary@thughunt.ing>
// SPDX-FileCopyrightText: 65 gus <august.eymann@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Actions;

namespace Content.Goobstation.Server.Actions;

public sealed partial class ActionsProviderSystem : EntitySystem
{
    [Dependency] private readonly SharedActionsSystem _actions = default!;
    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<ActionsProviderComponent, ComponentInit>(OnInit);
    }

    private void OnInit(Entity<ActionsProviderComponent> ent, ref ComponentInit args)
    {
        foreach (var action in ent.Comp.Actions)
            _actions.AddAction(ent, action);
    }
}