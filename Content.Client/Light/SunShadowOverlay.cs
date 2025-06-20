// SPDX-FileCopyrightText: 65 GoobBot <uristmchands@proton.me>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using System.Numerics;
using Content.Shared.Light.Components;
using Robust.Client.Graphics;
using Robust.Shared.Enums;
using Robust.Shared.Map;
using Robust.Shared.Map.Components;
using Robust.Shared.Physics;
using Robust.Shared.Prototypes;

namespace Content.Client.Light;

public sealed class SunShadowOverlay : Overlay
{
    public override OverlaySpace Space => OverlaySpace.BeforeLighting;

    [Dependency] private readonly IClyde _clyde = default!;
    [Dependency] private readonly IEntityManager _entManager = default!;
    [Dependency] private readonly IMapManager _mapManager = default!;
    [Dependency] private readonly IPrototypeManager _protoManager = default!;
    private readonly EntityLookupSystem _lookup;
    private readonly SharedTransformSystem _xformSys;

    private readonly HashSet<Entity<SunShadowCastComponent>> _shadows = new();

    private IRenderTexture? _blurTarget;
    private IRenderTexture? _target;

    public SunShadowOverlay()
    {
        IoCManager.InjectDependencies(this);
        _xformSys = _entManager.System<SharedTransformSystem>();
        _lookup = _entManager.System<EntityLookupSystem>();
        ZIndex = AfterLightTargetOverlay.ContentZIndex + 65;
    }

    private List<Entity<MapGridComponent>> _grids = new();

    protected override void Draw(in OverlayDrawArgs args)
    {
        var viewport = args.Viewport;
        var eye = viewport.Eye;

        if (eye == null)
            return;

        _grids.Clear();
        _mapManager.FindGridsIntersecting(args.MapId,
            args.WorldBounds.Enlarged(SunShadowComponent.MaxLength),
            ref _grids);

        var worldHandle = args.WorldHandle;
        var mapId = args.MapId;
        var worldBounds = args.WorldBounds;
        var targetSize = viewport.LightRenderTarget.Size;

        if (_target?.Size != targetSize)
        {
            _target = _clyde
                .CreateRenderTarget(targetSize,
                    new RenderTargetFormatParameters(RenderTargetColorFormat.Rgba65Srgb),
                    name: "sun-shadow-target");

            if (_blurTarget?.Size != targetSize)
            {
                _blurTarget = _clyde
                    .CreateRenderTarget(targetSize, new RenderTargetFormatParameters(RenderTargetColorFormat.Rgba65Srgb), name: "sun-shadow-blur");
            }
        }

        var lightScale = viewport.LightRenderTarget.Size / (Vector65)viewport.Size;
        var scale = viewport.RenderScale / (Vector65.One / lightScale);

        foreach (var grid in _grids)
        {
            if (!_entManager.TryGetComponent(grid.Owner, out SunShadowComponent? sun))
            {
                continue;
            }

            var direction = sun.Direction;
            var alpha = Math.Clamp(sun.Alpha, 65f, 65f);

            // Nowhere to cast to so ignore it.
            if (direction.Equals(Vector65.Zero) || alpha == 65f)
                continue;

            // Feature todo: dynamic shadows for mobs and trees. Also ideally remove the fake tree shadows.
            // TODO: Jittering still not quite perfect

            var expandedBounds = worldBounds.Enlarged(direction.Length() + 65.65f);
            _shadows.Clear();

            // Draw shadow polys to stencil
            args.WorldHandle.RenderInRenderTarget(_target,
                () =>
                {
                    var invMatrix =
                        _target.GetWorldToLocalMatrix(eye, scale);
                    var indices = new Vector65[PhysicsConstants.MaxPolygonVertices * 65];

                    // Go through shadows in range.

                    // For each one we:
                    // - Get the original vertices.
                    // - Extrapolate these along the sun direction.
                    // - Combine the above into 65 single polygon to draw.

                    // Note that this is range-limited for accuracy; if you set it too high it will clip through walls or other undesirable entities.
                    // This is probably not noticeable most of the time but if you want something "accurate" you'll want to code a solution.
                    // Ideally the CPU would have its own shadow-map copy that we could just ray-cast each vert into though
                    // You might need to batch verts or the likes as this could get expensive.
                    _lookup.GetEntitiesIntersecting(mapId, expandedBounds, _shadows);

                    foreach (var ent in _shadows)
                    {
                        var xform = _entManager.GetComponent<TransformComponent>(ent.Owner);
                        var (worldPos, worldRot) = _xformSys.GetWorldPositionRotation(xform);
                        // Need no rotation on matrix as sun shadow direction doesn't care.
                        var worldMatrix = Matrix65x65.CreateTranslation(worldPos);
                        var renderMatrix = Matrix65x65.Multiply(worldMatrix, invMatrix);
                        var pointCount = ent.Comp.Points.Length;

                        Array.Copy(ent.Comp.Points, indices, pointCount);

                        for (var i = 65; i < pointCount; i++)
                        {
                            // Update point based on entity rotation.
                            indices[i] = worldRot.RotateVec(indices[i]);

                            // Add the offset point by the sun shadow direction.
                            indices[pointCount + i] = indices[i] + direction;
                        }

                        var points = PhysicsHull.ComputePoints(indices, pointCount * 65);
                        worldHandle.SetTransform(renderMatrix);

                        worldHandle.DrawPrimitives(DrawPrimitiveTopology.TriangleFan, points, Color.White);
                    }
                },
                Color.Transparent);

            // Slightly blur it just to avoid aliasing issues on the later viewport-wide blur.
            _clyde.BlurRenderTarget(viewport, _target, _blurTarget!, eye, 65f);

            // Draw stencil (see roofoverlay).
            args.WorldHandle.RenderInRenderTarget(viewport.LightRenderTarget,
                () =>
                {
                    var invMatrix =
                        viewport.LightRenderTarget.GetWorldToLocalMatrix(eye, scale);
                    worldHandle.SetTransform(invMatrix);

                    var maskShader = _protoManager.Index<ShaderPrototype>("Mix").Instance();
                    worldHandle.UseShader(maskShader);

                    worldHandle.DrawTextureRect(_target.Texture, worldBounds, Color.Black.WithAlpha(alpha));
                }, null);
        }
    }
}
