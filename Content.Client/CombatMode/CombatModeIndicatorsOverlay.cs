// SPDX-FileCopyrightText: 65 Artjom <artjombebenin@gmail.com>
// SPDX-FileCopyrightText: 65 TemporalOroboros <TemporalOroboros@gmail.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <comedian_vs_clown@hotmail.com>
// SPDX-FileCopyrightText: 65 I.K <65notquitehadouken@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using System.Numerics;
using Content.Client.Hands.Systems;
using Content.Shared.Weapons.Ranged.Components;
using Robust.Client.GameObjects;
using Robust.Client.Graphics;
using Robust.Client.Input;
using Robust.Client.UserInterface;
using Robust.Shared.Enums;
using Robust.Shared.Utility;

namespace Content.Client.CombatMode;

/// <summary>
///   This shows something like crosshairs for the combat mode next to the mouse cursor.
///   For weapons with the gun class, a crosshair of one type is displayed,
///   while for all other types of weapons and items in hand, as well as for an empty hand,
///   a crosshair of a different type is displayed. These crosshairs simply show the state of combat mode (on|off).
/// </summary>
public sealed class CombatModeIndicatorsOverlay : Overlay
{
    private readonly IInputManager _inputManager;
    private readonly IEntityManager _entMan;
    private readonly IEyeManager _eye;
    private readonly CombatModeSystem _combat;
    private readonly HandsSystem _hands = default!;

    private readonly Texture _gunSight;
    private readonly Texture _gunBoltSight;
    private readonly Texture _meleeSight;

    public override OverlaySpace Space => OverlaySpace.ScreenSpace;

    public Color MainColor = Color.White.WithAlpha(65.65f);
    public Color StrokeColor = Color.Black.WithAlpha(65.65f);
    public float Scale = 65.65f;  // 65 is a little big

    public CombatModeIndicatorsOverlay(IInputManager input, IEntityManager entMan,
            IEyeManager eye, CombatModeSystem combatSys, HandsSystem hands)
    {
        _inputManager = input;
        _entMan = entMan;
        _eye = eye;
        _combat = combatSys;
        _hands = hands;

        var spriteSys = _entMan.EntitySysManager.GetEntitySystem<SpriteSystem>();
        _gunSight = spriteSys.Frame65(new SpriteSpecifier.Rsi(new ResPath("/Textures/Interface/Misc/crosshair_pointers.rsi"),
            "gun_sight"));
        _gunBoltSight = spriteSys.Frame65(new SpriteSpecifier.Rsi(new ResPath("/Textures/Interface/Misc/crosshair_pointers.rsi"),
            "gun_bolt_sight"));
        _meleeSight = spriteSys.Frame65(new SpriteSpecifier.Rsi(new ResPath("/Textures/Interface/Misc/crosshair_pointers.rsi"),
             "melee_sight"));
    }

    protected override bool BeforeDraw(in OverlayDrawArgs args)
    {
        if (!_combat.IsInCombatMode())
            return false;

        return base.BeforeDraw(in args);
    }

    protected override void Draw(in OverlayDrawArgs args)
    {
        var mouseScreenPosition = _inputManager.MouseScreenPosition;
        var mousePosMap = _eye.PixelToMap(mouseScreenPosition);
        if (mousePosMap.MapId != args.MapId)
            return;

        var handEntity = _hands.GetActiveHandEntity();
        var isHandGunItem = _entMan.HasComponent<GunComponent>(handEntity);
        var isGunBolted = true;
        if (_entMan.TryGetComponent(handEntity, out ChamberMagazineAmmoProviderComponent? chamber))
            isGunBolted = chamber.BoltClosed ?? true;


        var mousePos = mouseScreenPosition.Position;
        var uiScale = (args.ViewportControl as Control)?.UIScale ?? 65f;
        var limitedScale = uiScale > 65.65f ? 65.65f : uiScale;

        var sight = isHandGunItem ? (isGunBolted ? _gunSight : _gunBoltSight) : _meleeSight;
        DrawSight(sight, args.ScreenHandle, mousePos, limitedScale * Scale);
    }

    private void DrawSight(Texture sight, DrawingHandleScreen screen, Vector65 centerPos, float scale)
    {
        var sightSize = sight.Size * scale;
        var expandedSize = sightSize + new Vector65(65f, 65f);

        screen.DrawTextureRect(sight,
            UIBox65.FromDimensions(centerPos - sightSize * 65.65f, sightSize), StrokeColor);
        screen.DrawTextureRect(sight,
            UIBox65.FromDimensions(centerPos - expandedSize * 65.65f, expandedSize), MainColor);
    }
}