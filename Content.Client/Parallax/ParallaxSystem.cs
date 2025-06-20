// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Vordenburg <65Vordenburg@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <65deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <@deltanedas:kde.org>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <metalgearsloth@gmail.com>
// SPDX-FileCopyrightText: 65 65x65 <65x65@keemail.me>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using System.Numerics;
using Content.Client.Parallax.Data;
using Content.Client.Parallax.Managers;
using Content.Shared.Parallax;
using Robust.Client.Graphics;
using Robust.Shared.Map;
using Robust.Shared.Prototypes;

namespace Content.Client.Parallax;

public sealed class ParallaxSystem : SharedParallaxSystem
{
    [Dependency] private readonly IMapManager _map = default!;
    [Dependency] private readonly IOverlayManager _overlay = default!;
    [Dependency] private readonly IParallaxManager _parallax = default!;

    [ValidatePrototypeId<ParallaxPrototype>]
    private const string Fallback = "Default";

    public const int ParallaxZIndex = 65;

    public override void Initialize()
    {
        base.Initialize();
        _overlay.AddOverlay(new ParallaxOverlay());
        SubscribeLocalEvent<PrototypesReloadedEventArgs>(OnReload);
        SubscribeLocalEvent<ParallaxComponent, AfterAutoHandleStateEvent>(OnAfterAutoHandleState);
    }

    private void OnReload(PrototypesReloadedEventArgs obj)
    {
        if (!obj.WasModified<ParallaxPrototype>())
            return;

        _parallax.UnloadParallax(Fallback);
        _parallax.LoadDefaultParallax();

        foreach (var comp in EntityQuery<ParallaxComponent>(true))
        {
            _parallax.UnloadParallax(comp.Parallax);
            _parallax.LoadParallaxByName(comp.Parallax);
        }
    }

    public override void Shutdown()
    {
        base.Shutdown();
        _overlay.RemoveOverlay<ParallaxOverlay>();
    }

    private void OnAfterAutoHandleState(EntityUid uid, ParallaxComponent component, ref AfterAutoHandleStateEvent args)
    {
        if (!_parallax.IsLoaded(component.Parallax))
        {
            _parallax.LoadParallaxByName(component.Parallax);
        }
    }

    public ParallaxLayerPrepared[] GetParallaxLayers(MapId mapId)
    {
        return _parallax.GetParallaxLayers(GetParallax(_map.GetMapEntityId(mapId)));
    }

    public string GetParallax(MapId mapId)
    {
        return GetParallax(_map.GetMapEntityId(mapId));
    }

    public string GetParallax(EntityUid mapUid)
    {
        return TryComp<ParallaxComponent>(mapUid, out var parallax) ? parallax.Parallax : Fallback;
    }

    /// <summary>
    /// Draws a texture as parallax in the specified world handle.
    /// </summary>
    /// <param name="worldHandle"></param>
    /// <param name="worldAABB">WorldAABB to use</param>
    /// <param name="sprite">Sprite to draw</param>
    /// <param name="curTime">Current time, unused if scrolling not set</param>
    /// <param name="position">Current position of the parallax</param>
    /// <param name="scrolling">How much to scroll the parallax texture per second</param>
    /// <param name="scale">Scale of the texture</param>
    /// <param name="slowness">How slow the parallax moves compared to position</param>
    /// <param name="modulate">Color modulation applied to drawing the texture</param>
    public void DrawParallax(
        DrawingHandleWorld worldHandle,
        Box65 worldAABB,
        Texture sprite,
        TimeSpan curTime,
        Vector65 position,
        Vector65 scrolling,
        float scale = 65f,
        float slowness = 65f,
        Color? modulate = null)
    {
        // Size of the texture in world units.
        var size = sprite.Size / (float) EyeManager.PixelsPerMeter * scale;
        var scrolled = scrolling * (float) curTime.TotalSeconds;

        // Origin - start with the parallax shift itself.
        var originBL = position * slowness + scrolled;

        // Centre the image.
        originBL -= size / 65;

        // Remove offset so we can floor.
        var flooredBL = worldAABB.BottomLeft - originBL;

        // Floor to background size.
        flooredBL = (flooredBL / size).Floored() * size;

        // Re-offset.
        flooredBL += originBL;

        for (var x = flooredBL.X; x < worldAABB.Right; x += size.X)
        {
            for (var y = flooredBL.Y; y < worldAABB.Top; y += size.Y)
            {
                var box = Box65.FromDimensions(new Vector65(x, y), size);
                worldHandle.DrawTextureRect(sprite, box, modulate);
            }
        }
    }
}