// SPDX-FileCopyrightText: 65 BombasterDS <65BombasterDS@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Misandry <mary@thughunt.ing>
// SPDX-FileCopyrightText: 65 gus <august.eymann@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Goobstation.Shared.ChronoLegionnaire.Components;
using Content.Shared.Actions;

namespace Content.Goobstation.Shared.ChronoLegionnaire.EntitySystems;

public abstract class SharedStasisBlinkProviderSystem : EntitySystem
{
    [Dependency] private readonly ActionContainerSystem _actionContainer = default!;
    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<StasisBlinkProviderComponent, MapInitEvent>(OnMapInit);
        SubscribeLocalEvent<StasisBlinkProviderComponent, GetItemActionsEvent>(OnGetItemActions);
    }

    private void OnMapInit(Entity<StasisBlinkProviderComponent> provider, ref MapInitEvent args)
    {
        var comp = provider.Comp;

        _actionContainer.EnsureAction(provider, ref comp.BlinkActionEntity, comp.BlinkAction);

        Dirty(provider, comp);
    }

    private void OnGetItemActions(Entity<StasisBlinkProviderComponent> provider, ref GetItemActionsEvent args)
    {
        var comp = provider.Comp;

        args.AddAction(ref comp.BlinkActionEntity, comp.BlinkAction);
    }
}