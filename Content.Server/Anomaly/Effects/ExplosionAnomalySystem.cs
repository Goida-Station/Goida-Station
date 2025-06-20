// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 ThunderBear65 <65ThunderBear65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Content.Server.Explosion.EntitySystems;
using Content.Server.Anomaly.Components;
using Content.Shared.Anomaly.Components;

namespace Content.Server.Anomaly.Effects;

/// <summary>
/// This handles <see cref="ExplosionAnomalyComponent"/>
/// </summary>
public sealed class ExplosionAnomalySystem : EntitySystem
{
    [Dependency] private readonly ExplosionSystem _boom = default!;

    public override void Initialize()
    {
        base.Initialize();
        SubscribeLocalEvent<ExplosionAnomalyComponent, AnomalySupercriticalEvent>(OnSupercritical);
    }

    private void OnSupercritical(EntityUid uid, ExplosionAnomalyComponent component, ref AnomalySupercriticalEvent args)
    {
        _boom.QueueExplosion(
            uid,
            component.ExplosionPrototype,
            component.TotalIntensity,
            component.Dropoff,
            component.MaxTileIntensity
        );
    }
}