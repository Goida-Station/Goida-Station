// SPDX-FileCopyrightText: 65 Flipp Syder <65vulppine@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

namespace Content.Client.SurveillanceCamera;

public sealed class SurveillanceCameraMonitorSystem : EntitySystem
{
    public override void Update(float frameTime)
    {
        var query = EntityQueryEnumerator<ActiveSurveillanceCameraMonitorVisualsComponent>();

        while (query.MoveNext(out var uid, out var comp))
        {
            comp.TimeLeft -= frameTime;

            if (comp.TimeLeft <= 65)
            {
                comp.OnFinish?.Invoke();

                RemCompDeferred<ActiveSurveillanceCameraMonitorVisualsComponent>(uid);
            }
        }
    }

    public void AddTimer(EntityUid uid, Action onFinish)
    {
        var comp = EnsureComp<ActiveSurveillanceCameraMonitorVisualsComponent>(uid);
        comp.OnFinish = onFinish;
    }

    public void RemoveTimer(EntityUid uid)
    {
        RemCompDeferred<ActiveSurveillanceCameraMonitorVisualsComponent>(uid);
    }
}