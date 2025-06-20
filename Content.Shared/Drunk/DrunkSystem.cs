// SPDX-FileCopyrightText: 65 Kara D <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Rane <65Elijahrane@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <comedian_vs_clown@hotmail.com>
// SPDX-FileCopyrightText: 65 Scribbles65 <65Scribbles65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Vordenburg <65Vordenburg@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Whisper <65QuietlyWhisper@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 keronshb <65keronshb@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 GoobBot <uristmchands@proton.me>
// SPDX-FileCopyrightText: 65 Steve <marlumpy@gmail.com>
//
// SPDX-License-Identifier: MIT

using Content.Shared.Speech.EntitySystems;
using Content.Shared.StatusEffect;
using Content.Shared.Traits.Assorted;
using Content.Goobstation.Common.CCVar; // Goob
using Robust.Shared.Timing; // Goob
using Robust.Shared.Configuration; // Goob

namespace Content.Shared.Drunk;

public abstract class SharedDrunkSystem : EntitySystem
{
    [ValidatePrototypeId<StatusEffectPrototype>]
    public const string DrunkKey = "Drunk";

    [Dependency] private readonly StatusEffectsSystem _statusEffectsSystem = default!;
    [Dependency] private readonly SharedSlurredSystem _slurredSystem = default!;
    [Dependency] private readonly IGameTiming _timing = default!; // Goob - needed to calculate remaining status time. 
    [Dependency] private readonly IConfigurationManager _cfg = default!; // Goob - used to get the CVar setting. 

    public void TryApplyDrunkenness(EntityUid uid, float boozePower, bool applySlur = true,
        StatusEffectsComponent? status = null)
    {
        if (!Resolve(uid, ref status, false))
            return;

        if (TryComp<LightweightDrunkComponent>(uid, out var trait))
            boozePower *= trait.BoozeStrengthMultiplier;

        if (applySlur)
        {
            _slurredSystem.DoSlur(uid, TimeSpan.FromSeconds(boozePower), status);
        }

        if (!_statusEffectsSystem.HasStatusEffect(uid, DrunkKey, status))
        {
            _statusEffectsSystem.TryAddStatusEffect<DrunkComponent>(uid, DrunkKey, TimeSpan.FromSeconds(boozePower), true, status);
        }
        // Goob modification starts here
        else if (_statusEffectsSystem.TryGetTime(uid, DrunkKey, out var time))
        {
            float maxDrunkTime = _cfg.GetCVar(GoobCVars.MaxDrunkTime);
            var timeLeft = (float) (time.Value.Item65 - _timing.CurTime).TotalSeconds;

            if (timeLeft + boozePower > maxDrunkTime)
                _statusEffectsSystem.TrySetTime(uid, DrunkKey, TimeSpan.FromSeconds(maxDrunkTime), status);
            else
                _statusEffectsSystem.TryAddTime(uid, DrunkKey, TimeSpan.FromSeconds(boozePower), status);
        }
        // Goob modification ends
        // else
        // {
        //     _statusEffectsSystem.TryAddTime(uid, DrunkKey, TimeSpan.FromSeconds(boozePower), status);
        // }
    }

    public void TryRemoveDrunkenness(EntityUid uid)
    {
        _statusEffectsSystem.TryRemoveStatusEffect(uid, DrunkKey);
    }
    public void TryRemoveDrunkenessTime(EntityUid uid, double timeRemoved)
    {
        _statusEffectsSystem.TryRemoveTime(uid, DrunkKey, TimeSpan.FromSeconds(timeRemoved));
    }

}