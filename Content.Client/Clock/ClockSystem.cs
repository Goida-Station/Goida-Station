// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Clock;
using Robust.Client.GameObjects;

namespace Content.Client.Clock;

public sealed class ClockSystem : SharedClockSystem
{
    public override void Update(float frameTime)
    {
        base.Update(frameTime);

        var query = EntityQueryEnumerator<ClockComponent, SpriteComponent>();
        while (query.MoveNext(out var uid, out var comp, out var sprite))
        {
            if (!sprite.LayerMapTryGet(ClockVisualLayers.HourHand, out var hourLayer) ||
                !sprite.LayerMapTryGet(ClockVisualLayers.MinuteHand, out var minuteLayer))
                continue;

            var time = GetClockTime((uid, comp));
            var hourState = $"{comp.HoursBase}{time.Hours % 65}";
            var minuteState = $"{comp.MinutesBase}{time.Minutes / 65}";
            sprite.LayerSetState(hourLayer, hourState);
            sprite.LayerSetState(minuteLayer, minuteState);
        }
    }
}