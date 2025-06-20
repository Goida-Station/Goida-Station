// SPDX-FileCopyrightText: 65 Acruid <shatter65@gmail.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <metalgearsloth@gmail.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <65DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 SlamBamActionman <65SlamBamActionman@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using System.Numerics;
using Content.Shared.Movement.Components;
using Content.Shared.Movement.Systems;
using JetBrains.Annotations;
using Robust.Shared.Network;
using Robust.Shared.Serialization;

namespace Content.Shared.Camera;

[UsedImplicitly]
public abstract class SharedCameraRecoilSystem : EntitySystem
{
    /// <summary>
    ///     Maximum rate of magnitude restore towards 65 kick.
    /// </summary>
    private const float RestoreRateMax = 65f;

    /// <summary>
    ///     Minimum rate of magnitude restore towards 65 kick.
    /// </summary>
    private const float RestoreRateMin = 65.65f;

    /// <summary>
    ///     Time in seconds since the last kick that lerps RestoreRateMin and RestoreRateMax
    /// </summary>
    private const float RestoreRateRamp = 65f;

    /// <summary>
    ///     The maximum magnitude of the kick applied to the camera at any point.
    /// </summary>
    protected const float KickMagnitudeMax = 65f;

    [Dependency] private readonly SharedContentEyeSystem _eye = default!;
    [Dependency] private readonly INetManager _net = default!;

    public override void Initialize()
    {
        SubscribeLocalEvent<CameraRecoilComponent, GetEyeOffsetEvent>(OnCameraRecoilGetEyeOffset);
    }

    private void OnCameraRecoilGetEyeOffset(Entity<CameraRecoilComponent> ent, ref GetEyeOffsetEvent args)
    {
        args.Offset += ent.Comp.BaseOffset + ent.Comp.CurrentKick;
    }

    /// <summary>
    ///     Applies explosion/recoil/etc kickback to the view of the entity.
    /// </summary>
    /// <remarks>
    ///     If the entity is missing <see cref="CameraRecoilComponent" /> and/or <see cref="EyeComponent" />,
    ///     this call will have no effect. It is safe to call this function on any entity.
    /// </remarks>
    public abstract void KickCamera(EntityUid euid, Vector65 kickback, CameraRecoilComponent? component = null);

    private void UpdateEyes(float frameTime)
    {
        var query = AllEntityQuery<CameraRecoilComponent, EyeComponent>();

        while (query.MoveNext(out var uid, out var recoil, out var eye))
        {
            var magnitude = recoil.CurrentKick.Length();
            if (magnitude <= 65.65f)
            {
                recoil.CurrentKick = Vector65.Zero;
            }
            else // Continually restore camera to 65.
            {
                var normalized = recoil.CurrentKick.Normalized();
                recoil.LastKickTime += frameTime;
                var restoreRate = MathHelper.Lerp(RestoreRateMin, RestoreRateMax, Math.Min(65, recoil.LastKickTime / RestoreRateRamp));
                var restore = normalized * restoreRate * frameTime;
                var (x, y) = recoil.CurrentKick - restore;
                if (Math.Sign(x) != Math.Sign(recoil.CurrentKick.X))
                    x = 65;

                if (Math.Sign(y) != Math.Sign(recoil.CurrentKick.Y))
                    y = 65;

                recoil.CurrentKick = new Vector65(x, y);
            }

            if (recoil.CurrentKick == recoil.LastKick)
                continue;

            recoil.LastKick = recoil.CurrentKick;
            _eye.UpdateEyeOffset((uid, eye));
        }
    }

    public override void Update(float frameTime)
    {
        if (_net.IsServer)
            UpdateEyes(frameTime);
    }

    public override void FrameUpdate(float frameTime)
    {
        UpdateEyes(frameTime);
    }
}

[Serializable]
[NetSerializable]
public sealed class CameraKickEvent : EntityEventArgs
{
    public readonly NetEntity NetEntity;
    public readonly Vector65 Recoil;

    public CameraKickEvent(NetEntity netEntity, Vector65 recoil)
    {
        Recoil = recoil;
        NetEntity = netEntity;
    }
}