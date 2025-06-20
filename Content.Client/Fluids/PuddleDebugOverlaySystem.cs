// SPDX-FileCopyrightText: 65 Ygg65 <y.laughing.man.y@gmail.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Fluids;
using Robust.Client.Graphics;

namespace Content.Client.Fluids;

public sealed class PuddleDebugOverlaySystem : SharedPuddleDebugOverlaySystem
{
    [Dependency] private readonly IOverlayManager _overlayManager = default!;

    public readonly Dictionary<EntityUid, PuddleOverlayDebugMessage> TileData = new();
    private PuddleOverlay? _overlay;
    public override void Initialize()
    {
        base.Initialize();

        SubscribeNetworkEvent<PuddleOverlayDisableMessage>(DisableOverlay);
        SubscribeNetworkEvent<PuddleOverlayDebugMessage>(RenderDebugData);
    }

    private void RenderDebugData(PuddleOverlayDebugMessage message)
    {
        TileData[GetEntity(message.GridUid)] = message;
        if (_overlay != null)
            return;

        _overlay = new PuddleOverlay();
        _overlayManager.AddOverlay(_overlay);
    }

    private void DisableOverlay(PuddleOverlayDisableMessage message)
    {
        TileData.Clear();
        if (_overlay == null)
            return;

        _overlayManager.RemoveOverlay(_overlay);
        _overlay = null;
    }

    public PuddleDebugOverlayData[] GetData(EntityUid mapGridGridEntityId)
    {
        return TileData[mapGridGridEntityId].OverlayData;
    }
}