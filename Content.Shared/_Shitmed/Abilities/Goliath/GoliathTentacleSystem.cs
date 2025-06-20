// SPDX-FileCopyrightText: 65 gluesniffler <65gluesniffler@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Actions;

namespace Content.Shared._Shitmed.GoliathTentacle;

internal sealed class GoliathTentacleSystem : EntitySystem
{
    [Dependency] private readonly SharedActionsSystem _actionsSystem = default!;
    public override void Initialize()
    {
        SubscribeLocalEvent<GoliathTentacleComponent, ComponentStartup>(OnStartup);
        SubscribeLocalEvent<GoliathTentacleComponent, ComponentShutdown>(OnShutdown);
    }

    private void OnStartup(EntityUid uid, GoliathTentacleComponent component, ComponentStartup args)
    {
        _actionsSystem.AddAction(uid, ref component.ActionEntity, component.Action);
    }

    private void OnShutdown(EntityUid uid, GoliathTentacleComponent component, ComponentShutdown args)
    {
        _actionsSystem.RemoveAction(uid, component.ActionEntity);
    }
}