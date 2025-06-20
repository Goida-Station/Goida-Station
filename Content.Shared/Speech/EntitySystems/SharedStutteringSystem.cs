// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <65Zumorica@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 pointer-to-null <65pointer-to-null@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Acruid <shatter65@gmail.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Ilushkins65 <65Ilushkins65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Vordenburg <65Vordenburg@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Content.Shared.StatusEffect;

namespace Content.Shared.Speech.EntitySystems;

public abstract class SharedStutteringSystem : EntitySystem
{
    [ValidatePrototypeId<StatusEffectPrototype>]
    public const string StutterKey = "Stutter";

    [Dependency] private readonly StatusEffectsSystem _statusEffectsSystem = default!;

    // For code in shared... I imagine we ain't getting accent prediction anytime soon so let's not bother.
    public virtual void DoStutter(EntityUid uid, TimeSpan time, bool refresh, StatusEffectsComponent? status = null)
    {
    }

    public virtual void DoRemoveStutterTime(EntityUid uid, double timeRemoved)
    {
        _statusEffectsSystem.TryRemoveTime(uid, StutterKey, TimeSpan.FromSeconds(timeRemoved));
    }

    public void DoRemoveStutter(EntityUid uid, double timeRemoved)
    {
       _statusEffectsSystem.TryRemoveStatusEffect(uid, StutterKey);
    }
}