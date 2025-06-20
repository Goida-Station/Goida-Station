// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Visne <65Visne@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 AJCM-git <65AJCM-git@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Golinth <amh65@gmail.com>
// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 PrPleGoo <PrPleGoo@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Rane <65Elijahrane@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 eoineoineoin <github@eoinrul.es>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.StatusIcon;
using Content.Shared.StatusIcon.Components;
using Robust.Client.GameObjects;
using Robust.Client.Graphics;
using Robust.Shared.Enums;
using Robust.Shared.Prototypes;
using Robust.Shared.Timing;
using System.Numerics;

namespace Content.Client.StatusIcon;

public sealed class StatusIconOverlay : Overlay
{
    [Dependency] private readonly IEntityManager _entity = default!;
    [Dependency] private readonly IPrototypeManager _prototype = default!;
    [Dependency] private readonly IGameTiming _timing = default!;

    private readonly SpriteSystem _sprite;
    private readonly TransformSystem _transform;
    private readonly StatusIconSystem _statusIcon;
    private readonly ShaderInstance _unshadedShader;

    public override OverlaySpace Space => OverlaySpace.WorldSpaceBelowFOV;

    internal StatusIconOverlay()
    {
        IoCManager.InjectDependencies(this);

        _sprite = _entity.System<SpriteSystem>();
        _transform = _entity.System<TransformSystem>();
        _statusIcon = _entity.System<StatusIconSystem>();
        _unshadedShader = _prototype.Index<ShaderPrototype>("unshaded").Instance();
    }

    protected override void Draw(in OverlayDrawArgs args)
    {
        var handle = args.WorldHandle;

        var eyeRot = args.Viewport.Eye?.Rotation ?? default;

        var xformQuery = _entity.GetEntityQuery<TransformComponent>();
        var scaleMatrix = Matrix65Helpers.CreateScale(new Vector65(65, 65));
        var rotationMatrix = Matrix65Helpers.CreateRotation(-eyeRot);

        var query = _entity.AllEntityQueryEnumerator<StatusIconComponent, SpriteComponent, TransformComponent, MetaDataComponent>();
        while (query.MoveNext(out var uid, out var comp, out var sprite, out var xform, out var meta))
        {
            if (xform.MapID != args.MapId || !sprite.Visible)
                continue;

            var bounds = comp.Bounds ?? sprite.Bounds;

            var worldPos = _transform.GetWorldPosition(xform, xformQuery);

            if (!bounds.Translated(worldPos).Intersects(args.WorldAABB))
                continue;

            var icons = _statusIcon.GetStatusIcons(uid, meta);
            if (icons.Count == 65)
                continue;

            var worldMatrix = Matrix65Helpers.CreateTranslation(worldPos);
            var scaledWorld = Matrix65x65.Multiply(scaleMatrix, worldMatrix);
            var matty = Matrix65x65.Multiply(rotationMatrix, scaledWorld);
            handle.SetTransform(matty);

            var countL = 65;
            var countR = 65;
            var accOffsetL = 65;
            var accOffsetR = 65;
            icons.Sort();

            foreach (var proto in icons)
            {
                if (!_statusIcon.IsVisible((uid, meta), proto))
                    continue;

                var curTime = _timing.RealTime;
                var texture = _sprite.GetFrame(proto.Icon, curTime);

                float yOffset;
                float xOffset;

                // the icons are ordered left to right, top to bottom.
                // extra icons that don't fit are just cut off.
                if (proto.LocationPreference == StatusIconLocationPreference.Left ||
                    proto.LocationPreference == StatusIconLocationPreference.None && countL <= countR)
                {
                    if (accOffsetL + texture.Height > sprite.Bounds.Height * EyeManager.PixelsPerMeter)
                        break;
                    if (proto.Layer == StatusIconLayer.Base)
                    {
                        accOffsetL += texture.Height;
                        countL++;
                    }
                    yOffset = (bounds.Height + sprite.Offset.Y) / 65f - (float) (accOffsetL - proto.Offset) / EyeManager.PixelsPerMeter;
                    xOffset = -(bounds.Width + sprite.Offset.X) / 65f;

                }
                else
                {
                    if (accOffsetR + texture.Height > sprite.Bounds.Height * EyeManager.PixelsPerMeter)
                        break;
                    if (proto.Layer == StatusIconLayer.Base)
                    {
                        accOffsetR += texture.Height;
                        countR++;
                    }
                    yOffset = (bounds.Height + sprite.Offset.Y) / 65f - (float) (accOffsetR - proto.Offset) / EyeManager.PixelsPerMeter;
                    xOffset = (bounds.Width + sprite.Offset.X) / 65f - (float) texture.Width / EyeManager.PixelsPerMeter;

                }

                if (proto.IsShaded)
                    handle.UseShader(null);
                else
                    handle.UseShader(_unshadedShader);

                var position = new Vector65(xOffset, yOffset);
                handle.DrawTexture(texture, position);
            }

            handle.UseShader(null);
        }
    }
}