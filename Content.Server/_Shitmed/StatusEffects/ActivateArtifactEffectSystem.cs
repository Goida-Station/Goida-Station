// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 gluesniffler <65gluesniffler@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared._Shitmed.StatusEffects;
using Content.Server.Xenoarchaeology.Artifact;
using Content.Shared.Xenoarchaeology.Artifact.Components;
using Content.Shared.Coordinates;

namespace Content.Server._Shitmed.StatusEffects;

public sealed class ActivateArtifactEffectSystem : EntitySystem
{
    [Dependency] private readonly XenoArtifactSystem _artifact = default!;
    public override void Initialize()
    {
        SubscribeLocalEvent<ActivateArtifactEffectComponent, ComponentInit>(OnInit);
    }
    private void OnInit(EntityUid uid, ActivateArtifactEffectComponent component, ComponentInit args)
    {
        if (!TryComp<XenoArtifactComponent>(uid, out var artifact))
            return;

        _artifact.TryActivateXenoArtifact((uid, artifact), null, null, uid.ToCoordinates());
    }


}
