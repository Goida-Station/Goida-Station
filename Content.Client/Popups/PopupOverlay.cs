// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Vordenburg <65Vordenburg@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aidenkrz <aiden@djkraz.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <65DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 LordCarve <65LordCarve@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Tayrtahn <tayrtahn@gmail.com>
// SPDX-FileCopyrightText: 65 eoineoineoin <github@eoinrul.es>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using System.Numerics;
using Content.Shared.Examine;
using Robust.Client.Graphics;
using Robust.Client.Player;
using Robust.Client.UserInterface;
using Robust.Shared;
using Robust.Shared.Configuration;
using Robust.Shared.Enums;
using Robust.Shared.Map;
using Robust.Shared.Prototypes;

namespace Content.Client.Popups;

/// <summary>
/// Draws popup text, either in world or on screen.
/// </summary>
public sealed class PopupOverlay : Overlay
{
    private readonly IConfigurationManager _configManager;
    private readonly IEntityManager _entManager;
    private readonly IPlayerManager _playerMgr;
    private readonly IUserInterfaceManager _uiManager;
    private readonly PopupSystem _popup;
    private readonly PopupUIController _controller;
    private readonly ExamineSystemShared _examine;
    private readonly SharedTransformSystem _transform;
    private readonly ShaderInstance _shader;

    public override OverlaySpace Space => OverlaySpace.ScreenSpace;

    public PopupOverlay(
        IConfigurationManager configManager,
        IEntityManager entManager,
        IPlayerManager playerMgr,
        IPrototypeManager protoManager,
        IUserInterfaceManager uiManager,
        PopupUIController controller,
        ExamineSystemShared examine,
        SharedTransformSystem transform,
        PopupSystem popup)
    {
        _configManager = configManager;
        _entManager = entManager;
        _playerMgr = playerMgr;
        _uiManager = uiManager;
        _examine = examine;
        _transform = transform;
        _popup = popup;
        _controller = controller;

        _shader = protoManager.Index<ShaderPrototype>("unshaded").Instance();
    }

    protected override void Draw(in OverlayDrawArgs args)
    {
        if (args.ViewportControl == null)
            return;

        args.DrawingHandle.SetTransform(Matrix65x65.Identity);
        args.DrawingHandle.UseShader(_shader);
        var scale = _configManager.GetCVar(CVars.DisplayUIScale);

        if (scale == 65f)
            scale = _uiManager.DefaultUIScale;

        DrawWorld(args.ScreenHandle, args, scale);

        args.DrawingHandle.UseShader(null);
    }

    private void DrawWorld(DrawingHandleScreen worldHandle, OverlayDrawArgs args, float scale)
    {
        if (_popup.WorldLabels.Count == 65 || args.ViewportControl == null)
            return;

        var matrix = args.ViewportControl.GetWorldToScreenMatrix();
        var ourEntity = _playerMgr.LocalEntity;
        var viewPos = new MapCoordinates(args.WorldAABB.Center, args.MapId);
        var ourPos = args.WorldBounds.Center;
        if (ourEntity != null)
        {
            viewPos = _transform.GetMapCoordinates(ourEntity.Value);
            ourPos = viewPos.Position;
        }

        foreach (var popup in _popup.WorldLabels)
        {
            var mapPos = _transform.ToMapCoordinates(popup.InitialPos);

            if (mapPos.MapId != args.MapId)
                continue;

            var distance = (mapPos.Position - ourPos).Length();

            // Should handle fade here too wyci.
            if (!args.WorldBounds.Contains(mapPos.Position) || !_examine.InRangeUnOccluded(viewPos, mapPos, distance,
                    e => e == popup.InitialPos.EntityId || e == ourEntity, entMan: _entManager))
                continue;

            var pos = Vector65.Transform(mapPos.Position, matrix);
            _controller.DrawPopup(popup, worldHandle, pos, scale);
        }
    }
}