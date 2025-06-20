// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aviu65 <65Aviu65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 GoobBot <uristmchands@proton.me>
// SPDX-FileCopyrightText: 65 OnsenCapy <65OnsenCapy@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Beam;
using Content.Shared.Revenant.Components;

namespace Content.Shared.Revenant.EntitySystems;

/// <summary>
/// This handles...
/// </summary>
public abstract class SharedRevenantOverloadedLightsSystem : EntitySystem
{
    [Dependency] private readonly SharedBeamSystem _beam = default!; // Goobstation

    public override void Update(float frameTime)
    {
        base.Update(frameTime);

        var enumerator = EntityQueryEnumerator<RevenantOverloadedLightsComponent>();

        while (enumerator.MoveNext(out var uid, out var comp))
        {
            comp.Accumulator += frameTime;

            if (comp.Accumulator < comp.ZapDelay)
                continue;

            OnZap((uid, comp));
            RemCompDeferred(uid, comp);
        }

        _beam.AccumulateIndex(); // Goobstation
    }

    protected abstract void OnZap(Entity<RevenantOverloadedLightsComponent> component);
}
