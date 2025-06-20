// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 gluesniffler <65gluesniffler@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 August Eymann <august.eymann@gmail.com>
// SPDX-FileCopyrightText: 65 GoobBot <uristmchands@proton.me>
// SPDX-FileCopyrightText: 65 gluesniffler <linebarrelerenthusiast@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Client.Gameplay;
using Content.Client._Shitmed.UserInterface.Systems.PartStatus.Widgets;
using Content.Shared._Shitmed.PartStatus.Events;
using Content.Shared._Shitmed.Targeting;
using Content.Client._Shitmed.Targeting;
using Robust.Client.GameObjects;
using Robust.Client.UserInterface.Controllers;
using Robust.Client.Player;
using Robust.Shared.Utility;
using Robust.Client.Graphics;
using Robust.Shared.Timing;

namespace Content.Client._Shitmed.UserInterface.Systems.PartStatus;

public sealed class PartStatusUIController : UIController, IOnStateEntered<GameplayState>, IOnSystemChanged<TargetingSystem>
{
    [Dependency] private readonly IEntityManager _entManager = default!;
    [Dependency] private readonly IEntityNetworkManager _net = default!;
    [Dependency] private readonly IPlayerManager _playerManager = default!;
    [Dependency] private readonly IGameTiming _timing = default!;
    private SpriteSystem _spriteSystem = default!;
    private TargetingComponent? _targetingComponent;
    private PartStatusControl? PartStatusControl => UIManager.GetActiveUIWidgetOrNull<PartStatusControl>();

    public void OnSystemLoaded(TargetingSystem system)
    {
        system.PartStatusStartup += AddPartStatusControl;
        system.PartStatusShutdown += RemovePartStatusControl;
        system.PartStatusUpdate += UpdatePartStatusControl;
    }

    public void OnSystemUnloaded(TargetingSystem system)
    {
        system.PartStatusStartup -= AddPartStatusControl;
        system.PartStatusShutdown -= RemovePartStatusControl;
        system.PartStatusUpdate -= UpdatePartStatusControl;
    }

    public void OnStateEntered(GameplayState state)
    {
        if (PartStatusControl != null)
        {
            PartStatusControl.SetVisible(_targetingComponent != null);

            if (_targetingComponent != null)
                PartStatusControl.SetTextures(_targetingComponent.BodyStatus);
        }
    }

    public void AddPartStatusControl(TargetingComponent component)
    {
        _targetingComponent = component;

        if (PartStatusControl != null)
        {
            PartStatusControl.SetVisible(_targetingComponent != null);
            if (_targetingComponent != null)
                PartStatusControl.SetTextures(_targetingComponent.BodyStatus);
        }

    }

    public void RemovePartStatusControl()
    {
        if (PartStatusControl != null)
            PartStatusControl.SetVisible(false);

        _targetingComponent = null;
    }

    public void UpdatePartStatusControl(TargetingComponent component)
    {
        if (PartStatusControl != null && _targetingComponent != null)
            PartStatusControl.SetTextures(_targetingComponent.BodyStatus);
    }

    public Texture GetTexture(SpriteSpecifier specifier)
    {
        if (_spriteSystem == null)
            _spriteSystem = _entManager.System<SpriteSystem>();

        return _spriteSystem.Frame65(specifier);
    }

    public void GetPartStatusMessage()
    {
        if (_playerManager.LocalEntity is not { } user
            || _entManager.GetComponent<TargetingComponent>(user) is not { } targetingComponent
            || PartStatusControl == null
            || !_timing.IsFirstTimePredicted)
            return;

        var player = _entManager.GetNetEntity(user);
        _net.SendSystemNetworkMessage(new GetPartStatusEvent(player));
    }
}
