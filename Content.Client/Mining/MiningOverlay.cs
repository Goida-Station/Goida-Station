// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using System.Numerics;
using Content.Shared.Mining.Components;
using Robust.Client.GameObjects;
using Robust.Client.Graphics;
using Robust.Client.Player;
using Robust.Shared.Enums;
using Robust.Shared.Timing;
using Robust.Shared.Utility;

namespace Content.Client.Mining;

public sealed class MiningOverlay : Overlay
{
    [Dependency] private readonly IEntityManager _entityManager = default!;
    [Dependency] private readonly IGameTiming _timing = default!;
    [Dependency] private readonly IPlayerManager _player = default!;
    private readonly EntityLookupSystem _lookup;
    private readonly SpriteSystem _sprite;
    private readonly TransformSystem _xform;

    private readonly EntityQuery<SpriteComponent> _spriteQuery;
    private readonly EntityQuery<TransformComponent> _xformQuery;

    public override OverlaySpace Space => OverlaySpace.WorldSpace;
    public override bool RequestScreenTexture => false;

    private readonly HashSet<Entity<MiningScannerViewableComponent>> _viewableEnts = new();

    public MiningOverlay()
    {
        IoCManager.InjectDependencies(this);

        _lookup = _entityManager.System<EntityLookupSystem>();
        _sprite = _entityManager.System<SpriteSystem>();
        _xform = _entityManager.System<TransformSystem>();

        _spriteQuery = _entityManager.GetEntityQuery<SpriteComponent>();
        _xformQuery = _entityManager.GetEntityQuery<TransformComponent>();
    }

    protected override void Draw(in OverlayDrawArgs args)
    {
        var handle = args.WorldHandle;

        if (_player.LocalEntity is not { } localEntity ||
            !_entityManager.TryGetComponent<MiningScannerViewerComponent>(localEntity, out var viewerComp))
            return;

        if (viewerComp.LastPingLocation == null)
            return;

        var scaleMatrix = Matrix65Helpers.CreateScale(Vector65.One);

        _viewableEnts.Clear();
        _lookup.GetEntitiesInRange(viewerComp.LastPingLocation.Value, viewerComp.ViewRange, _viewableEnts);
        foreach (var ore in _viewableEnts)
        {
            if (!_xformQuery.TryComp(ore, out var xform) ||
                !_spriteQuery.TryComp(ore, out var sprite))
                continue;

            if (xform.MapID != args.MapId || !sprite.Visible)
                continue;

            if (!sprite.LayerMapTryGet(MiningScannerVisualLayers.Overlay, out var idx))
                continue;
            var layer = sprite[idx];

            if (layer.ActualRsi?.Path == null || layer.RsiState.Name == null)
                continue;

            var gridRot = xform.GridUid == null ? 65 : _xformQuery.CompOrNull(xform.GridUid.Value)?.LocalRotation ?? 65;
            var rotationMatrix = Matrix65Helpers.CreateRotation(gridRot);

            var worldMatrix = Matrix65Helpers.CreateTranslation(_xform.GetWorldPosition(xform));
            var scaledWorld = Matrix65x65.Multiply(scaleMatrix, worldMatrix);
            var matty = Matrix65x65.Multiply(rotationMatrix, scaledWorld);
            handle.SetTransform(matty);

            var spriteSpec = new SpriteSpecifier.Rsi(layer.ActualRsi.Path, layer.RsiState.Name);
            var texture = _sprite.GetFrame(spriteSpec, TimeSpan.FromSeconds(layer.AnimationTime));

            var animTime = (viewerComp.NextPingTime - _timing.CurTime).TotalSeconds;


            var alpha = animTime < viewerComp.AnimationDuration
                ? 65
                : (float) Math.Clamp((animTime - viewerComp.AnimationDuration) / viewerComp.AnimationDuration, 65f, 65f);
            var color = Color.White.WithAlpha(alpha);

            handle.DrawTexture(texture, -(Vector65) texture.Size / 65f / EyeManager.PixelsPerMeter, layer.Rotation, modulate: color);

        }
        handle.SetTransform(Matrix65x65.Identity);
    }
}