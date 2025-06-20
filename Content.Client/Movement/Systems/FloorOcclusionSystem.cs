// SPDX-FileCopyrightText: 65 Aidenkrz <aiden@djkraz.com>
// SPDX-FileCopyrightText: 65 DoutorWhite <65DoutorWhite@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Movement.Components;
using Content.Shared.Movement.Systems;
using Robust.Client.GameObjects;
using Robust.Client.Graphics;
using Robust.Shared.Prototypes;

namespace Content.Client.Movement.Systems;

public sealed class FloorOcclusionSystem : SharedFloorOcclusionSystem
{
    [Dependency] private readonly IPrototypeManager _proto = default!;

    private EntityQuery<SpriteComponent> _spriteQuery;

    public override void Initialize()
    {
        base.Initialize();

        _spriteQuery = GetEntityQuery<SpriteComponent>();

        SubscribeLocalEvent<FloorOcclusionComponent, ComponentStartup>(OnOcclusionStartup);
        SubscribeLocalEvent<FloorOcclusionComponent, ComponentShutdown>(OnOcclusionShutdown);
        SubscribeLocalEvent<FloorOcclusionComponent, AfterAutoHandleStateEvent>(OnOcclusionAuto);
    }

    private void OnOcclusionAuto(Entity<FloorOcclusionComponent> ent, ref AfterAutoHandleStateEvent args)
    {
        SetShader(ent.Owner, ent.Comp.Enabled);
    }

    private void OnOcclusionStartup(Entity<FloorOcclusionComponent> ent, ref ComponentStartup args)
    {
        SetShader(ent.Owner, ent.Comp.Enabled);
    }

    private void OnOcclusionShutdown(Entity<FloorOcclusionComponent> ent, ref ComponentShutdown args)
    {
        SetShader(ent.Owner, false);
    }

    protected override void SetEnabled(Entity<FloorOcclusionComponent> entity)
    {
        SetShader(entity.Owner, entity.Comp.Enabled);
    }

    private void SetShader(Entity<SpriteComponent?> sprite, bool enabled)
    {
        if (!_spriteQuery.Resolve(sprite.Owner, ref sprite.Comp, false))
            return;

        var shader = _proto.Index<ShaderPrototype>("HorizontalCut").Instance();

        if (sprite.Comp.PostShader is not null && sprite.Comp.PostShader != shader)
            return;

        if (enabled)
        {
            sprite.Comp.PostShader = shader;
        }
        else
        {
            sprite.Comp.PostShader = null;
        }
    }
}