// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Jezithyr <Jezithyr.@gmail.com>
// SPDX-FileCopyrightText: 65 Jezithyr <Jezithyr@gmail.com>
// SPDX-FileCopyrightText: 65 Jezithyr <jmaster65@gmail.com>
// SPDX-FileCopyrightText: 65 Visne <65Visne@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 wrexbe <wrexbe@protonmail.com>
// SPDX-FileCopyrightText: 65 Eoin Mcloughlin <helloworld@eoinrul.es>
// SPDX-FileCopyrightText: 65 Flipp Syder <65vulppine@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 eoineoineoin <eoin.mcloughlin+gh@gmail.com>
// SPDX-FileCopyrightText: 65 keronshb <keronshb@live.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <comedian_vs_clown@hotmail.com>
// SPDX-FileCopyrightText: 65 Aidenkrz <aiden@djkraz.com>
// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Client.Alerts;
using Content.Client.Gameplay;
using Content.Client.UserInterface.Systems.Alerts.Widgets;
using Content.Client.UserInterface.Systems.Gameplay;
using Content.Shared.Alert;
using Robust.Client.GameObjects;
using Robust.Client.Player;
using Robust.Client.UserInterface;
using Robust.Client.UserInterface.Controllers;
using Robust.Shared.Prototypes;

namespace Content.Client.UserInterface.Systems.Alerts;

public sealed class AlertsUIController : UIController, IOnStateEntered<GameplayState>, IOnSystemChanged<ClientAlertsSystem>
{
    [Dependency] private readonly IPlayerManager _player = default!;

    [UISystemDependency] private readonly ClientAlertsSystem? _alertsSystem = default;

    private AlertsUI? UI => UIManager.GetActiveUIWidgetOrNull<AlertsUI>();

    public override void Initialize()
    {
        base.Initialize();

        var gameplayStateLoad = UIManager.GetUIController<GameplayStateLoadController>();
        gameplayStateLoad.OnScreenLoad += OnScreenLoad;
        gameplayStateLoad.OnScreenUnload += OnScreenUnload;
    }

    private void OnScreenUnload()
    {
        var widget = UI;
        if (widget != null)
            widget.AlertPressed -= OnAlertPressed;
    }

    private void OnScreenLoad()
    {
        var widget = UI;
        if (widget != null)
            widget.AlertPressed += OnAlertPressed;

        SyncAlerts();
    }

    private void OnAlertPressed(object? sender, ProtoId<AlertPrototype> e)
    {
        _alertsSystem?.AlertClicked(e);
    }

    private void SystemOnClearAlerts(object? sender, EventArgs e)
    {
        UI?.ClearAllControls();
    }

    private void SystemOnSyncAlerts(object? sender, IReadOnlyDictionary<AlertKey, AlertState> e)
    {
        if (sender is ClientAlertsSystem system)
        {
            UI?.SyncControls(system, system.AlertOrder, e);
        }
    }

    public void OnSystemLoaded(ClientAlertsSystem system)
    {
        system.SyncAlerts += SystemOnSyncAlerts;
        system.ClearAlerts += SystemOnClearAlerts;
    }

    public void OnSystemUnloaded(ClientAlertsSystem system)
    {
        system.SyncAlerts -= SystemOnSyncAlerts;
        system.ClearAlerts -= SystemOnClearAlerts;
    }


    public void OnStateEntered(GameplayState state)
    {
        // initially populate the frame if system is available
        SyncAlerts();
    }

    public void SyncAlerts()
    {
        var alerts = _alertsSystem?.ActiveAlerts;
        if (alerts != null)
        {
            SystemOnSyncAlerts(_alertsSystem, alerts);
        }
    }

    public void UpdateAlertSpriteEntity(EntityUid spriteViewEnt, AlertPrototype alert)
    {
        if (_player.LocalEntity is not { } player)
            return;

        if (!EntityManager.TryGetComponent<SpriteComponent>(spriteViewEnt, out var sprite))
            return;

        var ev = new UpdateAlertSpriteEvent((spriteViewEnt, sprite), alert);
        EntityManager.EventBus.RaiseLocalEvent(player, ref ev);
    }
}