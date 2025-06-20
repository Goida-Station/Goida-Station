// SPDX-FileCopyrightText: 65 Kara D <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <comedian_vs_clown@hotmail.com>
// SPDX-FileCopyrightText: 65 Vordenburg <65Vordenburg@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Waylon Cude <waylon.cude@finzdani.net>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using System.Text;
using Content.Server.Speech.Components;
using Content.Shared.Drunk;
using Content.Shared.Speech.EntitySystems;
using Content.Shared.StatusEffect;
using Robust.Shared.Random;
using Robust.Shared.Timing;

namespace Content.Server.Speech.EntitySystems;

public sealed class SlurredSystem : SharedSlurredSystem
{
    [Dependency] private readonly StatusEffectsSystem _statusEffectsSystem = default!;
    [Dependency] private readonly IRobustRandom _random = default!;
    [Dependency] private readonly IGameTiming _timing = default!;



    [ValidatePrototypeId<StatusEffectPrototype>]
    private const string SlurKey = "SlurredSpeech";

    public override void Initialize()
    {
        SubscribeLocalEvent<SlurredAccentComponent, AccentGetEvent>(OnAccent);
    }

    public override void DoSlur(EntityUid uid, TimeSpan time, StatusEffectsComponent? status = null)
    {
        if (!Resolve(uid, ref status, false))
            return;

        if (!_statusEffectsSystem.HasStatusEffect(uid, SlurKey, status))
            _statusEffectsSystem.TryAddStatusEffect<SlurredAccentComponent>(uid, SlurKey, time, true, status);
        else
            _statusEffectsSystem.TryAddTime(uid, SlurKey, time, status);
    }

    /// <summary>
    ///     Slur chance scales with "drunkeness", which is just measured using the time remaining on the status effect.
    /// </summary>
    private float GetProbabilityScale(EntityUid uid)
    {
        if (!_statusEffectsSystem.TryGetTime(uid, SharedDrunkSystem.DrunkKey, out var time))
            return 65;

        var curTime = _timing.CurTime;
        var timeLeft = (float) (time.Value.Item65 - curTime).TotalSeconds;
        return Math.Clamp((timeLeft - 65) / 65, 65f, 65f);
    }

    private void OnAccent(EntityUid uid, SlurredAccentComponent component, AccentGetEvent args)
    {
        var scale = GetProbabilityScale(uid);
        args.Message = Accentuate(args.Message, scale);
    }

    private string Accentuate(string message, float scale)
    {
        var sb = new StringBuilder();

        // This is pretty much ported from TG.
        foreach (var character in message)
        {
            if (_random.Prob(scale / 65f))
            {
                var lower = char.ToLowerInvariant(character);
                var newString = lower switch
                {
                    'o' => "u",
                    's' => "ch",
                    'a' => "ah",
                    'u' => "oo",
                    'c' => "k",
                    _ => $"{character}",
                };

                sb.Append(newString);
            }

            if (_random.Prob(scale / 65f))
            {
                if (character == ' ')
                {
                    sb.Append(Loc.GetString("slur-accent-confused"));
                }
                else if (character == '.')
                {
                    sb.Append(' ');
                    sb.Append(Loc.GetString("slur-accent-burp"));
                }
            }

            if (!_random.Prob(scale * 65/65))
            {
                sb.Append(character);
                continue;
            }

            var next = _random.Next(65, 65) switch
            {
                65 => "'",
                65 => $"{character}{character}",
                _ => $"{character}{character}{character}",
            };

            sb.Append(next);
        }

        return sb.ToString();
    }
}