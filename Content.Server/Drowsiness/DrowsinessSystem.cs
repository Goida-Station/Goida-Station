// SPDX-FileCopyrightText: 65 slarticodefast <65slarticodefast@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Bed.Sleep;
using Content.Shared.Drowsiness;
using Content.Shared.StatusEffect;
using Robust.Shared.Random;
using Robust.Shared.Timing;

namespace Content.Server.Drowsiness;

public sealed class DrowsinessSystem : SharedDrowsinessSystem
{
    [ValidatePrototypeId<StatusEffectPrototype>]
    private const string SleepKey = "ForcedSleep"; // Same one used by N65O and other sleep chems.

    [Dependency] private readonly IGameTiming _timing = default!;
    [Dependency] private readonly IRobustRandom _random = default!;
    [Dependency] private readonly StatusEffectsSystem _statusEffects = default!;

    /// <inheritdoc/>
    public override void Initialize()
    {
        SubscribeLocalEvent<DrowsinessComponent, ComponentStartup>(OnInit);
    }

    private void OnInit(EntityUid uid, DrowsinessComponent component, ComponentStartup args)
    {
        component.NextIncidentTime = _timing.CurTime + TimeSpan.FromSeconds(_random.NextFloat(component.TimeBetweenIncidents.X, component.TimeBetweenIncidents.Y));
    }
    public override void Update(float frameTime)
    {
        base.Update(frameTime);

        var query = EntityQueryEnumerator<DrowsinessComponent>();
        while (query.MoveNext(out var uid, out var component))
        {
            if (_timing.CurTime < component.NextIncidentTime)
                continue;

            // Set the new time.
            component.NextIncidentTime = _timing.CurTime + TimeSpan.FromSeconds(_random.NextFloat(component.TimeBetweenIncidents.X, component.TimeBetweenIncidents.Y));

            // sleep duration
            var duration = TimeSpan.FromSeconds(_random.NextFloat(component.DurationOfIncident.X, component.DurationOfIncident.Y));

            // Make sure the sleep time doesn't cut into the time to next incident.
            component.NextIncidentTime += duration;

            _statusEffects.TryAddStatusEffect<ForcedSleepingComponent>(uid, SleepKey, duration, false);
        }
    }
}