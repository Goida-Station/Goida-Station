// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 gluesniffler <65gluesniffler@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared._Shitmed.StatusEffects;
using Content.Server.Teleportation;
using Content.Shared.Teleportation;

namespace Content.Server._Shitmed.StatusEffects;

public sealed class ScrambleLocationEffectSystem : EntitySystem
{
    [Dependency] private readonly TeleportSystem _teleportSys = default!;
    public override void Initialize()
    {
        SubscribeLocalEvent<ScrambleLocationEffectComponent, ComponentInit>(OnInit);
    }
    private void OnInit(EntityUid uid, ScrambleLocationEffectComponent component, ComponentInit args)
    {
        // TODO: Add the teleport component via onAdd:
        var teleport = EnsureComp<RandomTeleportComponent>(uid);
        _teleportSys.RandomTeleport(uid, teleport);
    }


}
