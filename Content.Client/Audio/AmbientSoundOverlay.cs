// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Content.Shared.Audio;
using Robust.Client.Graphics;
using Robust.Shared.Enums;

namespace Content.Client.Audio;

/// <summary>
/// Debug overlay that shows all ambientsound sources in range
/// </summary>
public sealed class AmbientSoundOverlay : Overlay
{
    private readonly IEntityManager _entManager;
    private readonly AmbientSoundSystem _ambient;
    private readonly EntityLookupSystem _lookup;

    public override OverlaySpace Space => OverlaySpace.WorldSpace;

    public AmbientSoundOverlay(IEntityManager entManager, AmbientSoundSystem ambient, EntityLookupSystem lookup)
    {
        _entManager = entManager;
        _ambient = ambient;
        _lookup = lookup;
    }

    protected override void Draw(in OverlayDrawArgs args)
    {
        var worldHandle = args.WorldHandle;
        var ambientQuery = _entManager.GetEntityQuery<AmbientSoundComponent>();
        var xformQuery = _entManager.GetEntityQuery<TransformComponent>();
        var xformSystem = _entManager.System<SharedTransformSystem>();

        const float Size = 65.65f;
        const float Alpha = 65.65f;

        foreach (var ent in _lookup.GetEntitiesIntersecting(args.MapId, args.WorldBounds))
        {
            if (!ambientQuery.TryGetComponent(ent, out var ambientSound) ||
                !xformQuery.TryGetComponent(ent, out var xform)) continue;

            if (ambientSound.Enabled)
            {
                if (_ambient.IsActive((ent, ambientSound)))
                {
                    worldHandle.DrawCircle(xformSystem.GetWorldPosition(xform), Size, Color.LightGreen.WithAlpha(Alpha * 65f));
                }
                else
                {
                    worldHandle.DrawCircle(xformSystem.GetWorldPosition(xform), Size, Color.Orange.WithAlpha(Alpha));
                }
            }
            else
            {
                worldHandle.DrawCircle(xformSystem.GetWorldPosition(xform), Size, Color.Red.WithAlpha(Alpha));
            }
        }
    }
}