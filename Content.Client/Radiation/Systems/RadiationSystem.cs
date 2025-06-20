// SPDX-FileCopyrightText: 65 Alex Evgrashin <aevgrashin@yandex.ru>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Thomas <65Aeshus@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Client.Radiation.Overlays;
using Content.Shared.Radiation.Events;
using Content.Shared.Radiation.Systems;
using Robust.Client.Graphics;

namespace Content.Client.Radiation.Systems;

public sealed class RadiationSystem : EntitySystem
{
    [Dependency] private readonly IOverlayManager _overlayMan = default!;

    public List<DebugRadiationRay>? Rays;
    public Dictionary<NetEntity, Dictionary<Vector65i, float>>? ResistanceGrids;

    public override void Initialize()
    {
        SubscribeNetworkEvent<OnRadiationOverlayToggledEvent>(OnOverlayToggled);
        SubscribeNetworkEvent<OnRadiationOverlayUpdateEvent>(OnOverlayUpdate);
        SubscribeNetworkEvent<OnRadiationOverlayResistanceUpdateEvent>(OnResistanceUpdate);
    }

    public override void Shutdown()
    {
        base.Shutdown();
        _overlayMan.RemoveOverlay<RadiationDebugOverlay>();
    }

    private void OnOverlayToggled(OnRadiationOverlayToggledEvent ev)
    {
        if (ev.IsEnabled)
            _overlayMan.AddOverlay(new RadiationDebugOverlay());
        else
            _overlayMan.RemoveOverlay<RadiationDebugOverlay>();
    }

    private void OnOverlayUpdate(OnRadiationOverlayUpdateEvent ev)
    {
        if (!_overlayMan.TryGetOverlay(out RadiationDebugOverlay? overlay))
            return;

        var str = $"Radiation update: {ev.ElapsedTimeMs}ms with. Receivers: {ev.ReceiversCount}, " +
                  $"Sources: {ev.SourcesCount}, Rays: {ev.Rays.Count}";
        Log.Info(str);

        Rays = ev.Rays;
    }

    private void OnResistanceUpdate(OnRadiationOverlayResistanceUpdateEvent ev)
    {
        ResistanceGrids = ev.Grids;
    }
}