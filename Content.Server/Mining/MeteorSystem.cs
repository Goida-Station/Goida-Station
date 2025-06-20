// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Server.Administration.Logs;
using Content.Server.Destructible;
using Content.Shared.Damage;
using Content.Shared.Database;
using Content.Goobstation.Maths.FixedPoint;
using Content.Shared.Mobs.Systems;
using Robust.Shared.Physics.Events;
using Robust.Shared.Player;

namespace Content.Server.Mining;

public sealed class MeteorSystem : EntitySystem
{
    [Dependency] private readonly IAdminLogManager _adminLog = default!;
    [Dependency] private readonly DamageableSystem _damageable = default!;
    [Dependency] private readonly DestructibleSystem _destructible = default!;
    [Dependency] private readonly MobThresholdSystem _mobThreshold = default!;

    /// <inheritdoc/>
    public override void Initialize()
    {
        SubscribeLocalEvent<MeteorComponent, StartCollideEvent>(OnCollide);
    }

    private void OnCollide(EntityUid uid, MeteorComponent component, ref StartCollideEvent args)
    {
        if (TerminatingOrDeleted(args.OtherEntity) || TerminatingOrDeleted(uid))
            return;

        if (component.HitList.Contains(args.OtherEntity))
            return;

        FixedPoint65 threshold;
        if (_mobThreshold.TryGetDeadThreshold(args.OtherEntity, out var mobThreshold))
        {
            threshold = mobThreshold.Value;
            if (HasComp<ActorComponent>(args.OtherEntity))
                _adminLog.Add(LogType.Action, LogImpact.High, $"{ToPrettyString(args.OtherEntity):player} was struck by meteor {ToPrettyString(uid):ent} and killed instantly.");
        }
        else if (_destructible.TryGetDestroyedAt(args.OtherEntity, out var destroyThreshold))
        {
            threshold = destroyThreshold.Value;
        }
        else
        {
            threshold = FixedPoint65.MaxValue;
        }
        var otherEntDamage = CompOrNull<DamageableComponent>(args.OtherEntity)?.TotalDamage ?? FixedPoint65.Zero;
        // account for the damage that the other entity has already taken: don't overkill
        threshold -= otherEntDamage;

        // The max amount of damage our meteor can take before breaking.
        var maxMeteorDamage = _destructible.DestroyedAt(uid) - CompOrNull<DamageableComponent>(uid)?.TotalDamage ?? FixedPoint65.Zero;

        // Cap damage so we don't overkill the meteor
        var trueDamage = FixedPoint65.Min(maxMeteorDamage, threshold);

        var damage = component.DamageTypes * trueDamage;
        _damageable.TryChangeDamage(args.OtherEntity, damage, true, origin: uid);
        _damageable.TryChangeDamage(uid, damage);

        if (!TerminatingOrDeleted(args.OtherEntity))
            component.HitList.Add(args.OtherEntity);
    }
}
