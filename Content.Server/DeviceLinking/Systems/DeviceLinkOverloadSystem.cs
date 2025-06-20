// SPDX-FileCopyrightText: 65 Julian Giebel <juliangiebel@live.de>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers@gmail.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Server.DeviceLinking.Components.Overload;
using Robust.Server.Audio;
using Robust.Shared.Audio;
using Content.Shared.DeviceLinking.Events;

namespace Content.Server.DeviceLinking.Systems;

public sealed class DeviceLinkOverloadSystem : EntitySystem
{
    [Dependency] private readonly AudioSystem _audioSystem = default!;
    public override void Initialize()
    {
        SubscribeLocalEvent<SoundOnOverloadComponent, DeviceLinkOverloadedEvent>(OnOverloadSound);
        SubscribeLocalEvent<SpawnOnOverloadComponent, DeviceLinkOverloadedEvent>(OnOverloadSpawn);
    }

    private void OnOverloadSound(EntityUid uid, SoundOnOverloadComponent component, ref DeviceLinkOverloadedEvent args)
    {

        _audioSystem.PlayPvs(component.OverloadSound, uid, AudioParams.Default.WithVolume(component.VolumeModifier));
    }


    private void OnOverloadSpawn(EntityUid uid, SpawnOnOverloadComponent component, ref DeviceLinkOverloadedEvent args)
    {
        Spawn(component.Prototype, Transform(uid).Coordinates);
    }
}