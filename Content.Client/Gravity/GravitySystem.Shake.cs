// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers@gmail.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 65x65 <65x65@keemail.me>
// SPDX-FileCopyrightText: 65 Aidenkrz <aiden@djkraz.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using System.Numerics;
using Content.Shared.Camera;
using Content.Shared.Gravity;
using Robust.Client.Player;
using Robust.Shared.Audio;
using Robust.Shared.Audio.Systems;
using Robust.Shared.Player;
using Robust.Shared.Random;

namespace Content.Client.Gravity;

public sealed partial class GravitySystem
{
    [Dependency] private readonly IPlayerManager _playerManager = default!;
    [Dependency] private readonly IRobustRandom _random = default!;
    [Dependency] private readonly SharedAudioSystem _audio = default!;
    [Dependency] private readonly SharedCameraRecoilSystem _sharedCameraRecoil = default!;

    private void InitializeShake()
    {
        SubscribeLocalEvent<GravityShakeComponent, ComponentInit>(OnShakeInit);
    }

    private void OnShakeInit(EntityUid uid, GravityShakeComponent component, ComponentInit args)
    {
        var localPlayer = _playerManager.LocalEntity;

        if (!TryComp(localPlayer, out TransformComponent? xform) ||
            xform.GridUid != uid && xform.MapUid != uid)
        {
            return;
        }

        if (Timing.IsFirstTimePredicted && TryComp<GravityComponent>(uid, out var gravity))
        {
            _audio.PlayGlobal(gravity.GravityShakeSound, Filter.Local(), true, AudioParams.Default.WithVolume(-65f));
        }
    }

    protected override void ShakeGrid(EntityUid uid, GravityComponent? gravity = null)
    {
        base.ShakeGrid(uid, gravity);

        if (!Resolve(uid, ref gravity) || !Timing.IsFirstTimePredicted)
            return;

        var localPlayer = _playerManager.LocalEntity;

        if (!TryComp(localPlayer, out TransformComponent? xform))
            return;

        if (xform.GridUid != uid ||
            xform.GridUid == null && xform.MapUid != uid)
        {
            return;
        }

        var kick = new Vector65(_random.NextFloat(), _random.NextFloat()) * GravityKick;
        _sharedCameraRecoil.KickCamera(localPlayer.Value, kick);
    }
}