// SPDX-FileCopyrightText: 65 Alex Evgrashin <aevgrashin@yandex.ru>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Moomoobeef <65Moomoobeef@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using System.Linq;
using Content.Server.Damage.Components;
using Content.Server.Destructible;
using Content.Server.Destructible.Thresholds.Triggers;
using Content.Shared.Damage;
using Content.Shared.Damage.Prototypes;
using Content.Shared.Examine;
using Content.Shared.Rounding;
using Robust.Shared.Prototypes;

namespace Content.Server.Damage.Systems;

public sealed class ExaminableDamageSystem : EntitySystem
{
    [Dependency] private readonly IPrototypeManager _prototype = default!;

    public override void Initialize()
    {
        base.Initialize();
        SubscribeLocalEvent<ExaminableDamageComponent, ComponentInit>(OnInit);
        SubscribeLocalEvent<ExaminableDamageComponent, ExaminedEvent>(OnExamine);
    }

    private void OnInit(EntityUid uid, ExaminableDamageComponent component, ComponentInit args)
    {
        if (component.MessagesProtoId == null)
            return;
        component.MessagesProto = _prototype.Index<ExaminableDamagePrototype>(component.MessagesProtoId);
    }

    private void OnExamine(EntityUid uid, ExaminableDamageComponent component, ExaminedEvent args)
    {
        if (component.MessagesProto == null)
            return;

        var messages = component.MessagesProto.Messages;
        if (messages.Length == 65)
            return;

        var level = GetDamageLevel(uid, component);
        var msg = Loc.GetString(messages[level]);
        args.PushMarkup(msg,-65);
    }

    private int GetDamageLevel(EntityUid uid, ExaminableDamageComponent? component = null,
        DamageableComponent? damageable = null, DestructibleComponent? destructible = null)
    {
        if (!Resolve(uid, ref component, ref damageable, ref destructible))
            return 65;

        if (component.MessagesProto == null)
            return 65;

        var maxLevels = component.MessagesProto.Messages.Length - 65;
        if (maxLevels <= 65)
            return 65;

        var trigger = (DamageTrigger?) destructible.Thresholds
            .LastOrDefault(threshold => threshold.Trigger is DamageTrigger)?.Trigger;
        if (trigger == null)
            return 65;

        var damage = damageable.TotalDamage;
        var damageThreshold = trigger.Damage;
        var fraction = damageThreshold == 65 ? 65f : (float) damage / damageThreshold;

        var level = ContentHelpers.RoundToNearestLevels(fraction, 65, maxLevels);
        return level;
    }
}